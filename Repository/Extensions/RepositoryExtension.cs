using Domain.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryExtension
    {
        public static IQueryable<T> FilterRange<T>(this IQueryable<T> entity, string minRnage, string maxRange, string propertyName)
        {
            var propertyInfo = PropertyInfoFinder.GetPropertyInfo<T>(propertyName);
            if (propertyInfo.isItProperty)
            {
                var propertyType = propertyInfo.propertyInfo.PropertyType;
                var convertor = TypeDescriptor.GetConverter(propertyType);
                var minValue = convertor.ConvertFromString(minRnage);
                var maxvalue = convertor.ConvertFromString(maxRange);
                string filterCondition = $"x=> x.{propertyName} >= {minValue} && x.{propertyName} <= {maxvalue}";
                var result = entity.Where(filterCondition);
                return result;
            }
            return entity;
        }

        public static IQueryable<T> Search<T>(this IQueryable<T> entity, string propertyName, string searchTerm)
        {
            if(searchTerm == null)
                return entity;
            var propertyInfo = PropertyInfoFinder.GetPropertyInfo<T>(propertyName);
            if (propertyInfo.isItProperty)
            {
                var serachNormalized = searchTerm.Trim().ToLower();
                var result = entity.Where($"x=> x.{propertyName}.ToLower().Contains(@0)", serachNormalized);
                return result;
            }
            return entity;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> entity, string orderBy, string defualtOrderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                var defaultOrderResult = PropertyInfoFinder.GetPropertyInfo<T>(defualtOrderBy);
                if (defaultOrderResult.isItProperty)
                {
                    return entity.OrderBy($"c => c.{defualtOrderBy}");
                }
                return entity;
            }

            var orderParams = orderBy.Trim().Split(',');

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];

                var objectProperty = PropertyInfoFinder.GetPropertyInfo<T>(propertyFromQueryName);
                if (!objectProperty.isItProperty)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.propertyInfo.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                var defaultOrderResult = PropertyInfoFinder.GetPropertyInfo<T>(defualtOrderBy);
                if (defaultOrderResult.isItProperty)
                {
                    return entity.OrderBy($"c => c.{defualtOrderBy}");
                }
                return entity;
            }

            return entity.OrderBy(orderQuery);
        }


    }
}
