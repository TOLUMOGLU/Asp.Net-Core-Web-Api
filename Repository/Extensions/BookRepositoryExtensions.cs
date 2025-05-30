﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions;

namespace Repository.EFCore
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
        {
            return books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);
        }

    
        public static IQueryable<Book> Search(this IQueryable<Book> books,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) 
                return books;
            var lowerCaseTerm  = searchTerm.Trim().ToLower(); //küçük harfe duyarsız
            return books
                .Where(b => b.Title
                .ToLower()
                .Contains(searchTerm));
        }


        public static IQueryable<Book> Sort(this IQueryable<Book> books,
            string orderByQueryString)
        {
            if (!string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return books.OrderBy(b=> b.Id);
            }
            //var orderParams = orderByQueryString.Split(','); //books?orderBy=title,price

            //var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //var orderQueryBuilder = new StringBuilder();

            //title ascending, price descending, id ascending[,]
            //foreach (var param in orderParams) 
            //{
            //    if(string.IsNullOrWhiteSpace(param))
            //        continue;
            //    var propertyFromQueryName = param.Split(' ')[0];//price desc gibi olunca 
            //    var objectProperty = propertyInfos
            //        .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
            //        StringComparison.InvariantCultureIgnoreCase));
            //    if (objectProperty is null)
            //        continue;
            //    var direction = param.EndsWith("des") ? "descending" : "ascending";
            //    orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");

            //}
            //var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book> (orderByQueryString);

            if(orderQuery is null)
                return books.OrderBy(b => b.Id);
                return books.OrderBy(b => b.Id);

            return books.OrderBy(orderQuery);

        }
    }
}