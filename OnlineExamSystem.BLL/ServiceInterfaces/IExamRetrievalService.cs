using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.Common.Entities;
using OnlineExamSystem.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.ServiceInterfaces
{
    public interface IExamRetrievalService
    {
        Task<ExamDto> GetExamByIdAsync(int id);
        Task<IReadOnlyList<ExamDto>> GetAllExamsAsync();
        Task<IReadOnlyList<ExamDto>> GetExamsWithSpecAsync(ISpecification<Exam> spec);
    }
}
