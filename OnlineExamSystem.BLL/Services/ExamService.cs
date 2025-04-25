using Microsoft.Extensions.Logging;
using OnlineExamSystem.Common.Models;
using OnlineExamSystem.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.BLL.Interfaces;

namespace OnlineExamSystem.BLL.Services
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamService> _logger;

        public ExamService(IUnitOfWork unitOfWork, ILogger<ExamService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Exam>> GetExamsWithQuestionsAsync()
        {
            return await _unitOfWork.Exams.GetExamsWithQuestionsAsync();
        }

        public async Task<Exam?> GetExamByIdAsync(int examId)
        {
            return await _unitOfWork.Exams.GetByIdAsync(examId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId)
        {
            var questions = await _unitOfWork.Questions.GetQuestionsByExamIdAsync(examId);
            if (questions == null || !questions.Any())
            {
                _logger.LogWarning($"No questions found for Exam ID {examId}.");
            }
            return questions ?? new List<Question>();
        }

        public async Task<SubmitExamResult> SubmitExamAsync(string userId, SubmitExamViewModel model)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(model.ExamId);
                if (exam == null)
                {
                    return new SubmitExamResult { Success = false, Message = "Exam not found." };
                }

                var questions = await _unitOfWork.Questions.GetQuestionsByExamIdAsync(model.ExamId);
                if (!questions.Any())
                {
                    return new SubmitExamResult { Success = false, Message = "No questions available." };
                }

                if (model.Answers == null || !model.Answers.Any())
                {
                    return new SubmitExamResult { Success = false, Message = "No answers provided." };
                }

                var existingUserExam = await _unitOfWork.UserExams.FindAsync(ue => ue.UserId == userId && ue.ExamId == model.ExamId);
                if (existingUserExam.Any())
                {
                    return new SubmitExamResult { Success = false, Message = "You have already submitted this exam." };
                }

                var userExam = new UserExam
                {
                    UserId = userId,
                    ExamId = model.ExamId,
                    TakenAt = DateTime.UtcNow
                };

                await _unitOfWork.UserExams.AddAsync(userExam);
                await _unitOfWork.CompleteAsync(); 

                int correctAnswers = 0;
                foreach (var question in questions)
                {
                    int selectedOption = 0;
                    bool hasAnswer = model.Answers.TryGetValue(question.Id, out selectedOption);

                    if (!hasAnswer || selectedOption < 1 || selectedOption > 4)
                    {
                        _logger.LogWarning(hasAnswer
                            ? $"Invalid selected option {selectedOption} for Question ID {question.Id}."
                            : $"No answer provided for Question ID {question.Id}.");
                        selectedOption = 0; // Use 0 to indicate no answer
                    }

                    bool isCorrect = selectedOption != 0 && selectedOption == question.CorrectOption;
                    if (isCorrect)
                    {
                        correctAnswers++;
                    }
                    else
                    {
                        _logger.LogInformation($"Incorrect answer for Question ID {question.Id}.");
                    }

                    var userAnswer = new UserAnswer
                    {
                        UserExamId = userExam.Id,
                        QuestionId = question.Id,
                        SelectedOption = selectedOption 
                    };
                    await _unitOfWork.UserAnswers.AddAsync(userAnswer);
                }

                int totalQuestions = questions.Count();
                userExam.Score = totalQuestions > 0 ? (correctAnswers * 100 / totalQuestions) : 0;
                userExam.IsPassed = userExam.Score >= 60;

                await _unitOfWork.UserExams.UpdateAsync(userExam);
                await _unitOfWork.CompleteAsync(); 

                return new SubmitExamResult
                {
                    Success = true,
                    RedirectUrl = $"/Exam/Result?userExamId={userExam.Id}"
                };
            }
            catch (Exception ex)
            {
                string errorMessage = BuildErrorMessage(ex);
                return new SubmitExamResult { Success = false, Message = "An unexpected error occurred: " + errorMessage };
            }
        }

        public async Task<ExamResultViewModel> GetExamResultAsync(string userId, int userExamId)
        {
            var userExam = (await _unitOfWork.UserExams.GetAllAsync())
                .FirstOrDefault(ue => ue.Id == userExamId);

            if (userExam == null)
            {
                throw new KeyNotFoundException("User exam not found.");
            }

            userExam.Exam = await _unitOfWork.Exams.GetByIdAsync(userExam.ExamId);
            if (userExam.Exam == null)
            {
                throw new KeyNotFoundException("Associated exam not found for this user exam.");
            }

            var questions = await _unitOfWork.Questions.GetQuestionsByExamIdAsync(userExam.ExamId);
            if (questions == null)
            {
                questions = new List<Question>();
            }

            var userAnswers = (await _unitOfWork.UserAnswers.GetUserAnswersByUserIdAsync(userId))
                .Where(ua => ua.UserExamId == userExamId)
                .ToList();

            int correctAnswers = userAnswers.Count(ua =>
            {
                var question = questions.FirstOrDefault(q => q.Id == ua.QuestionId);
                bool isCorrect = question != null && ua.SelectedOption != 0 && ua.SelectedOption == question.CorrectOption;
                return isCorrect;
            });

            var model = new ExamResultViewModel
            {
                UserExam = userExam,
                Questions = questions.ToList(),
                UserAnswers = userAnswers,
                CorrectAnswers = correctAnswers,
                TotalQuestions = questions.Count()
            };

            return model;
        }

        private string BuildErrorMessage(Exception ex)
        {
            string errorMessage = ex.Message;
            Exception innerEx = ex.InnerException;
            while (innerEx != null)
            {
                errorMessage += $" Inner Exception: {innerEx.Message}";
                innerEx = innerEx.InnerException;
            }
            return errorMessage;
        }
    }
}