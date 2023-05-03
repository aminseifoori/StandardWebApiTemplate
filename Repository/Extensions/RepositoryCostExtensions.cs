using Domain.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryCostExtensions
    {
        public static IQueryable<Cost> FilterCost(this IQueryable<Cost> costs, decimal minAmount, decimal maxAmount)
        {
            return costs.Where(c=> c.Amount >= minAmount && c.Amount <= maxAmount);
        }

        public static IQueryable<Cost> SearchCost(this IQueryable<Cost> costs, string serachTerm)
        {
            if (string.IsNullOrWhiteSpace(serachTerm))
            {
                return costs;
            }

            var serachNormalized = serachTerm.Trim().ToLower();

            return costs.Where(c=> c.Description.ToLower().Contains(serachNormalized));
        }

        public static IQueryable<Cost> Sort(this IQueryable<Cost> costs, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return costs.OrderBy(c=> c.Amount);
            }

            var orderParams = orderBy.Trim().Split(',');
            var propertyInfos = typeof(Cost).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach ( var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if(objectProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if(string.IsNullOrWhiteSpace(orderQuery))
            {
                return costs.OrderBy(c=> c.Amount);
            }

            return costs.OrderBy(orderQuery);
        }
    }
}
