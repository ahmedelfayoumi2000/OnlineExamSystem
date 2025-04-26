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
    public class ExamRetrievalService : IExamRetrievalService
    {
        private readonly IGenericRepository<Exam> _examRepository;
        private readonly IMapper _mapper;

        public ExamRetrievalService(IGenericRepository<Exam> examRepository, IMapper mapper)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ExamDto> GetExamByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Exam ID must be greater than zero.");

            var spec = new BaseSpecification<Exam>(e => e.Id == id);
            spec.AddInclude(e => e.Questions);
            var exam = await _examRepository.GetByIdWithSpecAsync(spec);

            return exam == null ? null : _mapper.Map<ExamDto>(exam);
        }

        public async Task<IReadOnlyList<ExamDto>> GetAllExamsAsync()
        {
            var exams = await _examRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ExamDto>>(exams);
        }

        public async Task<IReadOnlyList<ExamDto>> GetExamsWithSpecAsync(ISpecification<Exam> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var exams = await _examRepository.GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<ExamDto>>(exams);
        }
    }
}