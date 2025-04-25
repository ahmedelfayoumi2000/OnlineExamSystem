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
    public class UserAnswerRepository : Repository<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswersByUserIdAsync(string userId)
        {
            return await _context.UserAnswers
                .Include(ua => ua.UserExam)
                    .ThenInclude(ue => ue.User)
                .Include(ua => ua.Question)
                .Where(ua => ua.UserExam.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.UserAnswers
                .Include(ua => ua.UserExam)
                    .ThenInclude(ue => ue.User)
                .Include(ua => ua.Question)
                .Where(ua => ua.QuestionId == questionId)
                .ToListAsync();
        }
    }
}