﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using NLog.LayoutRenderers;
using Repository.Contracts;
using Services.Contracts;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BookDto> _shaper;
        private IRepositoryManager repositoryManager;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _shaper = shaper;
        }

        public BookManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper)
        {
            this.repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {

            if (!bookParameters.ValidPriceRange) 
            {
                throw new PriceOutofRangeBadRequestException();
            }
            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(bookParameters, trackChanges);

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
            
            var shapedData = _shaper.ShapeData(booksDto, bookParameters.Fields);

            return (shapedData, booksWithMetaData.MetaData);
        }


        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExits(id, trackChanges);
            ////check entity
            //var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            //if(entity is null)
            //{
            //    //string message = $"The book with id:{id} could not found";
            //    //_logger.LogInfo(message);
            //    //throw new Exception(message);
            //    throw new BookNotFoundException(id);
            //}
            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            //var book = await _manager.Book.GetOneBookByIdAsync(id,trackChanges);
            var book = await GetOneBookByIdAndCheckExits(id, trackChanges);
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }
            return _mapper.Map<BookDto>(book); //bookDto veri dönmesini sağladık
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            //check entity
            //var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            var entity = await GetOneBookByIdAndCheckExits(id, trackChanges);

            //if (entity is null)
            //{
            //    //string message = $"Book with id:{id} could not found";
            //    //_logger.LogInfo(message);
            //    //throw new Exception(message);
            //    throw new BookNotFoundException(id);
            //}
            ////Mapping var otomatik olmalı
            //entity.Title = book.Title;
            //entity.Price = book.Price;

            entity = _mapper.Map<Book>(bookDto);  //varlığı mapper ile kaydettik

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            //var book = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            var book = await GetOneBookByIdAndCheckExits(id, trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);
            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task<Book> GetOneBookByIdAndCheckExits(int id, bool trackChanges)
        {
            //check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }
            return entity;
        }

       
    }
}
