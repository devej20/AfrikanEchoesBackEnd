using AfrikanEchoes.Helpers;
using AfrikanEchoes.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Route("api/BookImage")]
    [ApiController]
    public class BookImagesApiController : ControllerBase
    {
        private IBookImagesService _bookImageService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BookImagesApiController(
            IBookImagesService bookImageService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _bookImageService = bookImageService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Retrieves book
        /// </summary>
        /// <returns>A response with books list</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookImages")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetBookImages()
        {
            var images = _bookImageService.GetAll();
            return Ok(images);
        }

        /// <summary>
        /// Retrieves book
        /// </summary>
        /// <returns>A response with books list</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookImagesByBook/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetBookImagesByBook([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var images = _bookImageService.GetByBook(id);

            return Ok(images);
        }

        /// <summary>
        /// Retrieves a book by ID
        /// </summary>
        /// <param name="id">Book id</param>
        /// <returns>A response with book</returns>
        /// <response code="200">Returns the books list</response>
        /// <response code="404">If book is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("BookImage/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookImages([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookImages = await _bookImageService.GetById(id);

            if (bookImages == null)
            {
                return NotFound();
            }

            return Ok(bookImages);
        }

    }
}
