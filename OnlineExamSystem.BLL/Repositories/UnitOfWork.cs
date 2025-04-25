using Microsoft.Extensions.Logging;
using OnlineExamSystem.BLL.Interfaces;
using OnlineExamSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;

        public IExamRepository Exams { get; }
        public IQuestionRepository Questions { get; }
        public IUserExamRepository UserExams { get; }
        public IUserAnswerRepository UserAnswers { get; }

        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;

            Exams = new ExamRepository(_context);
            Questions = new QuestionRepository(_context);
            UserExams = new UserExamRepository(_context);
            UserAnswers = new UserAnswerRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to the database.");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _logger.LogInformation("UnitOfWork disposed.");
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
