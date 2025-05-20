using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T> (String orderByQueryString)
        {
            var orderParams = orderByQueryString.Split(','); //books?orderBy=title,price

            var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(' ')[0];//price desc gibi olunca 
                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                    StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty is null)
                    continue;
                var direction = param.EndsWith("des") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");

            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            return orderQuery;  

        }
    }
}
