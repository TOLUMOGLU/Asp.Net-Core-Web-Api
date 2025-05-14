using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        //private readonly RepositoryContext _context;
        //private readonly IRepositoryManager _manager;
        private readonly IServiceManager _manager;
        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GeAllBooks()
        {
            var books = _manager.BookService.GetAllBook(false);
            return Ok(books);
           
        }

        [HttpGet("{id}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)
        {
            var book = _manager.BookService.GetOneBook(id, false);
            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            if (book is null)
            {
                return BadRequest();
            }
            _manager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            _manager.BookService.UpdateOneBook(id, book, true);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager.BookService.GetOneBook(id, true); //entity yoksagetonebookdan hata geliyor
            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, entity, true);
            //_manager.Save();
            return NoContent();
        }
    }
}
