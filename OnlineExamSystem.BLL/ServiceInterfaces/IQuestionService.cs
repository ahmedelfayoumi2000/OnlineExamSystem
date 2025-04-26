using OnlineExamSystem.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.ServiceInterfaces
{
    public interface IQuestionService
    {
        Task<QuestionDto> GetQuestionByIdAsync(int id);
        Task<IReadOnlyList<QuestionDto>> GetQuestionsByExamIdAsync(int examId);
        Task<QuestionDto> CreateQuestionAsync(QuestionDto questionDto);
        Task UpdateQuestionAsync(QuestionDto questionDto);
        Task DeleteQuestionAsync(int id);
    }
}
