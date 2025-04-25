using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OnlineExamSystem.BLL.Interfaces;
using OnlineExamSystem.Common.Models;
using OnlineExamSystem.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<AdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Exam>> GetAllExamsAsync()
        {
            _logger.LogInformation("Fetching all exams for admin.");
            return await _unitOfWork.Exams.GetAllAsync();
        }

        public async Task<Exam?> GetExamByIdAsync(int examId)
        {
            _logger.LogInformation($"Fetching exam with ID {examId} for admin.");
            return await _unitOfWork.Exams.GetByIdAsync(examId);
        }

        public async Task CreateExamAsync(CreateExamViewModel model)
        {
            var exam = new Exam
            {
                Title = model.Title,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Exams.AddAsync(exam);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Exam '{exam.Title}' created successfully.");
        }

        public async Task UpdateExamAsync(int examId, CreateExamViewModel model)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                _logger.LogWarning($"Exam with ID {examId} not found for update.");
                throw new KeyNotFoundException("Exam not found.");
            }

            exam.Title = model.Title;
            await _unitOfWork.Exams.UpdateAsync(exam);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Exam '{exam.Title}' updated successfully.");
        }

        public async Task DeleteExamAsync(int examId)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
            if (exam == null)
            {
                _logger.LogWarning($"Exam with ID {examId} not found for deletion.");
                throw new KeyNotFoundException("Exam not found.");
            }

            await _unitOfWork.Exams.DeleteAsync(exam);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Exam '{exam.Title}' deleted successfully.");
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId)
        {
            _logger.LogInformation($"Fetching questions for Exam ID {examId} for admin.");
            return await _unitOfWork.Questions.GetQuestionsByExamIdAsync(examId);
        }

        public async Task<Question?> GetQuestionByIdAsync(int questionId)
        {
            _logger.LogInformation($"Fetching question with ID {questionId} for admin.");
            return await _unitOfWork.Questions.GetByIdAsync(questionId);
        }

        public async Task CreateQuestionAsync(CreateQuestionViewModel model)
        {
            // Validate the model
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                _logger.LogWarning("Question title cannot be empty.");
                throw new ArgumentException("Question title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model.Option1) || string.IsNullOrWhiteSpace(model.Option2) ||
                string.IsNullOrWhiteSpace(model.Option3) || string.IsNullOrWhiteSpace(model.Option4))
            {
                _logger.LogWarning("All question options must be provided.");
                throw new ArgumentException("All question options must be provided.");
            }

            if (model.CorrectOption < 1 || model.CorrectOption > 4)
            {
                _logger.LogWarning($"Invalid correct option {model.CorrectOption} for question. Must be between 1 and 4.");
                throw new ArgumentException("Correct option must be between 1 and 4.");
            }

            // Check if the exam exists
            var exam = await _unitOfWork.Exams.GetByIdAsync(model.ExamId);
            if (exam == null)
            {
                _logger.LogWarning($"Exam with ID {model.ExamId} not found while creating question.");
                throw new KeyNotFoundException("Exam not found.");
            }

            var question = new Question
            {
                Title = model.Title,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                Option4 = model.Option4,
                CorrectOption = model.CorrectOption,
                ExamId = model.ExamId
            };

            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Question '{question.Title}' created successfully for Exam ID {question.ExamId}.");
        }

        public async Task UpdateQuestionAsync(int questionId, CreateQuestionViewModel model)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question with ID {questionId} not found for update.");
                throw new KeyNotFoundException("Question not found.");
            }

            // Validate the model
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                _logger.LogWarning("Question title cannot be empty.");
                throw new ArgumentException("Question title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model.Option1) || string.IsNullOrWhiteSpace(model.Option2) ||
                string.IsNullOrWhiteSpace(model.Option3) || string.IsNullOrWhiteSpace(model.Option4))
            {
                _logger.LogWarning("All question options must be provided.");
                throw new ArgumentException("All question options must be provided.");
            }

            if (model.CorrectOption < 1 || model.CorrectOption > 4)
            {
                _logger.LogWarning($"Invalid correct option {model.CorrectOption} for question. Must be between 1 and 4.");
                throw new ArgumentException("Correct option must be between 1 and 4.");
            }

            question.Title = model.Title;
            question.Option1 = model.Option1;
            question.Option2 = model.Option2;
            question.Option3 = model.Option3;
            question.Option4 = model.Option4;
            question.CorrectOption = model.CorrectOption;
            question.ExamId = model.ExamId;

            await _unitOfWork.Questions.UpdateAsync(question);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Question '{question.Title}' updated successfully for Exam ID {question.ExamId}.");
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question with ID {questionId} not found for deletion.");
                throw new KeyNotFoundException("Question not found.");
            }

            await _unitOfWork.Questions.DeleteAsync(question);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation($"Question '{question.Title}' deleted successfully.");
        }

        public async Task CreateUserAsync(CreateUserViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning($"Failed to create user '{user.Email}'. Errors: {errors}");
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "User");
            _logger.LogInformation($"User '{user.Email}' created successfully.");
        }
    }
}