using AutoMapper;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.Common.Entities;
using OnlineExamSystem.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Services
{
    public class ExamModificationService : IExamModificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExamModificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ExamDto> CreateExamAsync(ExamDto examDto)
        {
            if (examDto == null) throw new ArgumentNullException(nameof(examDto));
            if (string.IsNullOrWhiteSpace(examDto.Title)) throw new ArgumentException("Exam title cannot be empty.", nameof(examDto.Title));
            if (examDto.Duration <= 0) throw new ArgumentOutOfRangeException(nameof(examDto.Duration), "Duration must be greater than zero.");
            if (examDto.PassingScore < 0 || examDto.PassingScore > 100) throw new ArgumentOutOfRangeException(nameof(examDto.PassingScore), "Passing score must be between 0 and 100.");

            var exam = _mapper.Map<Exam>(examDto);
            exam.CreatedAt = DateTime.UtcNow;
            exam.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Exam>().AddAsync(exam);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ExamDto>(exam);
        }

        public async Task UpdateExamAsync(ExamDto examDto)
        {
            if (examDto == null) throw new ArgumentNullException(nameof(examDto));
            if (examDto.Id <= 0) throw new ArgumentOutOfRangeException(nameof(examDto.Id), "Exam ID must be greater than zero.");
            if (string.IsNullOrWhiteSpace(examDto.Title)) throw new ArgumentException("Exam title cannot be empty.", nameof(examDto.Title));
            if (examDto.Duration <= 0) throw new ArgumentOutOfRangeException(nameof(examDto.Duration), "Duration must be greater than zero.");
            if (examDto.PassingScore < 0 || examDto.PassingScore > 100) throw new ArgumentOutOfRangeException(nameof(examDto.PassingScore), "Passing score must be between 0 and 100.");

            var examRepo = _unitOfWork.Repository<Exam>();
            var existingExam = await examRepo.GetByIdAsync(examDto.Id);
            if (existingExam == null) throw new InvalidOperationException($"Exam with ID {examDto.Id} not found.");

            _mapper.Map(examDto, existingExam);
            existingExam.UpdatedAt = DateTime.UtcNow;

            examRepo.Update(existingExam);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteExamAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Exam ID must be greater than zero.");

            var examRepo = _unitOfWork.Repository<Exam>();
            var exam = await examRepo.GetByIdAsync(id);
            if (exam == null) throw new InvalidOperationException($"Exam with ID {id} not found.");

            examRepo.Delete(exam);
            await _unitOfWork.CompleteAsync();
        }
    }
}