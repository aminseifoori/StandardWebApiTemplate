using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
    public static class PropertyInfoFinder
    {
        public static (bool isItProperty, PropertyInfo propertyInfo) GetPropertyInfo<T>(string entityPropertyName)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var entityProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(entityPropertyName, StringComparison.InvariantCultureIgnoreCase));
            if(entityProperty == null)
            {
                return (isItProperty: false, propertyInfo: null);
            }
            return (isItProperty: true, propertyInfo: entityProperty);
        }
    }
}
