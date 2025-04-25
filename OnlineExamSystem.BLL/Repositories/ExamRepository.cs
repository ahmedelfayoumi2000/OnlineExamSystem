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
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Exam>> GetExamsWithQuestionsAsync()
        {
            return await _context.Exams
                .Include(e => e.Questions)
                .ToListAsync();
        }
    }
}