using AutoMapper;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.Common.Entities;
using OnlineExamSystem.Common.Interfaces;
using OnlineExamSystem.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Services
{
    public class UserExamService : IUserExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserExamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserExamDto> SubmitExamAsync(string userId, int examId, Dictionary<int, int> userAnswers)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            if (examId <= 0) throw new ArgumentOutOfRangeException(nameof(examId), "Exam ID must be greater than zero.");
            if (userAnswers == null || !userAnswers.Any()) throw new ArgumentException("User answers cannot be empty.", nameof(userAnswers));

            var examSpec = new BaseSpecification<Exam>(e => e.Id == examId);
            examSpec.AddInclude(e => e.Questions);
            var exam = await _unitOfWork.Repository<Exam>().GetByIdWithSpecAsync(examSpec);
            if (exam == null) throw new InvalidOperationException($"Exam with ID {examId} not found.");

            var questionsSpec = new BaseSpecification<Question>(q => q.ExamId == examId);
            questionsSpec.AddInclude(q => q.Options);
            var questions = await _unitOfWork.Repository<Question>().GetAllWithSpecAsync(questionsSpec);

            if (!questions.Any()) throw new InvalidOperationException($"No questions found for Exam with ID {examId}.");

            int correctAnswers = 0;
            foreach (var question in questions)
            {
                if (!userAnswers.ContainsKey(question.Id)) continue;

                var selectedOptionId = userAnswers[question.Id];
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null && selectedOptionId == correctOption.Id)
                {
                    correctAnswers++;
                }
            }

            int totalQuestions = questions.Count;
            int score = (correctAnswers * 100) / totalQuestions;
            bool passed = score >= exam.PassingScore;

            var userExam = new UserExam
            {
                UserId = userId,
                ExamId = examId,
                Score = score,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                Passed = passed,
                TakenAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<UserExam>().AddAsync(userExam);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserExamDto>(userExam);
        }

        public async Task<IReadOnlyList<UserExamDto>> GetUserExamsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            var spec = new BaseSpecification<UserExam>(ue => ue.UserId == userId);
            spec.AddInclude(ue => ue.User);
            spec.AddInclude(ue => ue.Exam);
            var userExams = await _unitOfWork.Repository<UserExam>().GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<UserExamDto>>(userExams);
        }

        public async Task<UserExamDto> GetUserExamByIdAsync(int userExamId)
        {
            if (userExamId <= 0) throw new ArgumentOutOfRangeException(nameof(userExamId), "UserExam ID must be greater than zero.");

            var spec = new BaseSpecification<UserExam>(ue => ue.Id == userExamId);
            spec.AddInclude(ue => ue.User);
            spec.AddInclude(ue => ue.Exam);
            var userExam = await _unitOfWork.Repository<UserExam>().GetByIdWithSpecAsync(spec);

            return userExam == null ? null : _mapper.Map<UserExamDto>(userExam);
        }
    }
}