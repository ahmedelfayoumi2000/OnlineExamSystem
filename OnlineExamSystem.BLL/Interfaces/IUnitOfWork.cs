using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IExamRepository Exams { get; }
        IQuestionRepository Questions { get; }
        IUserExamRepository UserExams { get; }
        IUserAnswerRepository UserAnswers { get; }

        Task<int> CompleteAsync();
    }
}
