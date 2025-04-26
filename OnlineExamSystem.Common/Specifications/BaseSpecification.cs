using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OnlineExamSystem.Common.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecification()
        {
            Includes = new List<Expression<Func<T, object>>>();
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            Includes = new List<Expression<Func<T, object>>>();
        }

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            if (includeExpression == null) throw new ArgumentNullException(nameof(includeExpression));
            Includes.Add(includeExpression);
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression ?? throw new ArgumentNullException(nameof(orderByExpression));
        }

        public void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression ?? throw new ArgumentNullException(nameof(orderByDescExpression));
        }

        public void ApplyPagination(int skip, int take)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip), "Skip must be non-negative.");
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), "Take must be greater than zero.");

            IsPagingEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}