using OnlineExamSystem.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Interfaces
{
    public interface IUserExamRepository : IRepository<UserExam>
    {
        Task<IEnumerable<UserExam>> GetUserExamsByUserIdAsync(string userId);
    }
}
