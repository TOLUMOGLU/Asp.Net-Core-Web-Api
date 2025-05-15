using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public IEnumerable<Book> GetAllBook(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBook(id, trackChanges);
            if(entity is null)
            {
                //string message = $"The book with id:{id} could not found";
                //_logger.LogInfo(message);
                //throw new Exception(message);
                throw new BookNotFoundException(id);
            }
            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public Book GetOneBook(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBook(id,trackChanges);
            if(book is null)
            {
                throw new BookNotFoundException(id);
            }
            return book;
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBook(id, trackChanges);
            if (entity is null)
            {
                //string message = $"Book with id:{id} could not found";
                //_logger.LogInfo(message);
                //throw new Exception(message);
                throw new BookNotFoundException(id);
            }
            //Mapping var otomatik olmalı
            //entity.Title = book.Title;
            //entity.Price = book.Price;

            entity = _mapper.Map<Book>(bookDto);  //varlığı mapper ile kaydettik

            _manager.Book.Update(entity);
            _manager.Save();
        }
    }
}
