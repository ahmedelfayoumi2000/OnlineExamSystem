using OnlineExamSystem.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.ServiceInterfaces
{
    public interface IExamModificationService
    {
        Task<ExamDto> CreateExamAsync(ExamDto examDto);
        Task UpdateExamAsync(ExamDto examDto);
        Task DeleteExamAsync(int id);
    }
}
