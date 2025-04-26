using AutoMapper;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.Common.Entities;
using OnlineExamSystem.Common.Interfaces;
using OnlineExamSystem.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Question ID must be greater than zero.");

            var spec = new BaseSpecification<Question>(q => q.Id == id);
            spec.AddInclude(q => q.Options);
            var question = await _unitOfWork.Repository<Question>().GetByIdWithSpecAsync(spec);

            return question == null ? null : _mapper.Map<QuestionDto>(question);
        }

        public async Task<IReadOnlyList<QuestionDto>> GetQuestionsByExamIdAsync(int examId)
        {
            if (examId <= 0) throw new ArgumentOutOfRangeException(nameof(examId), "Exam ID must be greater than zero.");

            var spec = new BaseSpecification<Question>(q => q.ExamId == examId);
            spec.AddInclude(q => q.Options);
            var questions = await _unitOfWork.Repository<Question>().GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<QuestionDto>>(questions);
        }

        public async Task<QuestionDto> CreateQuestionAsync(QuestionDto questionDto)
        {
            if (questionDto == null) throw new ArgumentNullException(nameof(questionDto));
            if (string.IsNullOrWhiteSpace(questionDto.Text)) throw new ArgumentException("Question text cannot be empty.", nameof(questionDto.Text));
            if (questionDto.ExamId <= 0) throw new ArgumentOutOfRangeException(nameof(questionDto.ExamId), "Exam ID must be greater than zero.");
            if (questionDto.Options == null || questionDto.Options.Count != 4) throw new ArgumentException("Question must have exactly 4 options.", nameof(questionDto.Options));
            if (questionDto.Options.Count(o => o.IsCorrect) != 1) throw new ArgumentException("Question must have exactly one correct option.", nameof(questionDto.Options));
            if (questionDto.Options.Any(o => string.IsNullOrWhiteSpace(o.Text))) throw new ArgumentException("All options must have non-empty text.", nameof(questionDto.Options));

            var question = _mapper.Map<Question>(questionDto);
            await _unitOfWork.Repository<Question>().AddAsync(question);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<QuestionDto>(question);
        }

        public async Task UpdateQuestionAsync(QuestionDto questionDto)
        {
            if (questionDto == null) throw new ArgumentNullException(nameof(questionDto));
            if (questionDto.Id <= 0) throw new ArgumentOutOfRangeException(nameof(questionDto.Id), "Question ID must be greater than zero.");
            if (string.IsNullOrWhiteSpace(questionDto.Text)) throw new ArgumentException("Question text cannot be empty.", nameof(questionDto.Text));
            if (questionDto.ExamId <= 0) throw new ArgumentOutOfRangeException(nameof(questionDto.ExamId), "Exam ID must be greater than zero.");
            if (questionDto.Options == null || questionDto.Options.Count != 4) throw new ArgumentException("Question must have exactly 4 options.", nameof(questionDto.Options));
            if (questionDto.Options.Count(o => o.IsCorrect) != 1) throw new ArgumentException("Question must have exactly one correct option.", nameof(questionDto.Options));
            if (questionDto.Options.Any(o => string.IsNullOrWhiteSpace(o.Text))) throw new ArgumentException("All options must have non-empty text.", nameof(questionDto.Options));

            var questionRepo = _unitOfWork.Repository<Question>();
            var existingQuestion = await questionRepo.GetByIdAsync(questionDto.Id);
            if (existingQuestion == null) throw new InvalidOperationException($"Question with ID {questionDto.Id} not found.");

            _mapper.Map(questionDto, existingQuestion);
            questionRepo.Update(existingQuestion);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Question ID must be greater than zero.");

            var questionRepo = _unitOfWork.Repository<Question>();
            var question = await questionRepo.GetByIdAsync(id);
            if (question == null) throw new InvalidOperationException($"Question with ID {id} not found.");

            questionRepo.Delete(question);
            await _unitOfWork.CompleteAsync();
        }
    }
}