using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Common.Entities;
using OnlineExamSystem.Common.Interfaces;
using OnlineExamSystem.Common.Specifications; 
using OnlineExamSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamSystem.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            return await ApplySpecifications(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            return await ApplySpecifications(spec).CountAsync();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
        }

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
        }
    }
}