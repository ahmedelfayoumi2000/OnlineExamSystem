using OnlineExamSystem.Common.Models;
using OnlineExamSystem.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Exam>> GetAllExamsAsync();
        Task<Exam?> GetExamByIdAsync(int examId);
        Task CreateExamAsync(CreateExamViewModel model);
        Task UpdateExamAsync(int examId, CreateExamViewModel model);
        Task DeleteExamAsync(int examId);
        Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId);
        Task CreateQuestionAsync(CreateQuestionViewModel model);
        Task<Question?> GetQuestionByIdAsync(int questionId);
        Task UpdateQuestionAsync(int questionId, CreateQuestionViewModel model);
        Task DeleteQuestionAsync(int questionId);
        Task CreateUserAsync(CreateUserViewModel model);
    }
}
