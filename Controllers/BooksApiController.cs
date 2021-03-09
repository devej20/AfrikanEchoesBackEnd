using AfricanEchoes.Models;
using AfrikanEchoes.Entities;
using AfrikanEchoes.Helpers;
using AfrikanEchoes.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Route("api/Book")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private IBookService _bookService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private AfrikanEchoesDbContext _context;

        public BooksApiController(
            IBookService bookService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            AfrikanEchoesDbContext context)
        {
            _bookService = bookService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _context = context;
        }

        /// <summary>
        /// Retrieves book
        /// </summary>
        /// <returns>A response with books list</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Books")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetBook()
        {
            var books = _bookService.GetAll();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a book by ID
        /// </summary>
        /// <param name="id">Book id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Book/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBook([FromRoute] long id)
        {

            var response = new SingleResponse<Book>();
            try
            {
                var category = _bookService.GetById(id);
                var categoryModel = _mapper.Map<Book>(category);
                response.Model = categoryModel;
            }
            catch
            {
                response.Error = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves a book by Category ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookByCategory/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookByCategory([FromRoute] long id)
        {

            var books = _bookService.GetByCategory(id);
            return Ok(books);

        }

        /// <summary>
        /// Retrieves a book by Category ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookByLanguage/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookByLanguage([FromRoute] long id)
        {
            var books = await _context.Books.Where(x => x.LanguageId == id)
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages).ToListAsync();
            return Ok(books);

        }

        /// <summary>
        /// Retrieves a book by Category ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookByFeautured")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookByFeatured()
        {
            var books = _bookService.GetBookByFeautured();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a book by Category ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookByNew")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookByNew()
        {
            var books = _bookService.GetBookByNew();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a book by Category ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookByTopChart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookByTopChart()
        {
            var books = _bookService.GetBookByTopChart();
            return Ok(books);
        }

    }
}
