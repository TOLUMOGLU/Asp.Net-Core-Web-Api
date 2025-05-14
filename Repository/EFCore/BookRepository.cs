using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Repository.Contracts;

namespace Repository.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
            
        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges).OrderBy(x => x.Id);

        public Book GetOneBook(int id, bool trackChanges) => FindByCondition(b => b.Id.Equals(id),trackChanges).SingleOrDefault();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
