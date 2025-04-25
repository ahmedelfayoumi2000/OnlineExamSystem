using OnlineExamSystem.Common.Models;
using OnlineExamSystem.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<Exam>> GetExamsWithQuestionsAsync();
        Task<Exam?> GetExamByIdAsync(int examId);
        Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId);
        Task<SubmitExamResult> SubmitExamAsync(string userId, SubmitExamViewModel model);
        Task<ExamResultViewModel> GetExamResultAsync(string userId, int userExamId);
    }

    public class SubmitExamResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
