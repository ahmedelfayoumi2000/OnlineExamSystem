using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.BLL.Interfaces;
using OnlineExamSystem.Common.Models;
using OnlineExamSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Repositories
{
    public class UserExamRepository : Repository<UserExam>, IUserExamRepository
    {
        public UserExamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserExam>> GetUserExamsByUserIdAsync(string userId)
        {
            return await _context.UserExams
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Exam)
                .ToListAsync();
        }
    }

}

