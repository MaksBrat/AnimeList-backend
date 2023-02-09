using System.Linq.Expressions;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Services.Extentions;

namespace AnimeList.Common.Filters.Base
{
    public class BaseFilter<T> where T : class
    {
        public string? SearchQuery { get; set; }
        public string? OrderBy { get; set; }
        public string? AscOrDesc { get; set; }
        public int Take { get; set; } = 0;

        public Expression<Func<T, bool>>? Predicate { get; set; } = null;
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByQuery { get; set; } = null;

        protected void ApplySearchQueryFilter(string propertyName)
        {
            if (SearchQuery != null)
            {
                var parameterExp = Expression.Parameter(typeof(T), "a");
                var propertyExp = Expression.Property(parameterExp, propertyName);

                var containsMethodExp = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsExp = Expression.Call(propertyExp, containsMethodExp, Expression.Constant(SearchQuery));
      
                Predicate = Expression.Lambda<Func<T, bool>>(containsExp, parameterExp);
            }
        }

        protected void ApplyEnumFilter<E>(string enumValue, string propertyName) where E : struct
        {
            if (enumValue != null)
            {
                var parsedEnumValue = Enum.Parse<E>(enumValue);
                var parameterExpression = Expression.Parameter(typeof(T), "x");
                var propertyExpression = Expression.Property(parameterExpression, propertyName);
                var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(parsedEnumValue));

                Predicate = Predicate == null 
                    ? Expression.Lambda<Func<T, bool>>(equalExpression, parameterExpression) 
                    : Predicate.And(Expression.Lambda<Func<T, bool>>(equalExpression, parameterExpression));
            }
        }

        protected void ApplyOrderByFilter(string propertyName, string ascOrDesc) 
        {
            if (propertyName != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "t");
                Expression property = Expression.Property(param, propertyName);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

                if (ascOrDesc == "ASC")
                {
                    OrderByQuery = x => x.OrderBy(lambda);
                }
                else if (ascOrDesc == "DESC")
                {
                    OrderByQuery = x => x.OrderByDescending(lambda);
                }
            }
        }
    }
}
