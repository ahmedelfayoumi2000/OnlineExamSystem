using OnlineExamSystem.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Interfaces
{
    public interface IUserAnswerRepository : IRepository<UserAnswer>
    {
        Task<IEnumerable<UserAnswer>> GetUserAnswersByUserIdAsync(string userId);
        Task<IEnumerable<UserAnswer>> GetUserAnswersByQuestionIdAsync(int questionId);
    }
}
