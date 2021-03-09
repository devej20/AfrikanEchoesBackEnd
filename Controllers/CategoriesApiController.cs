using AfricanEchoes.Models;
using AfrikanEchoes.Helpers;
using AfrikanEchoes.Models.Categories;
using AfrikanEchoes.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CategoriesApiController(
            ICategoryService categoryService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Retrieves category
        /// </summary>
        /// <returns>A response with categorys list</returns>
        /// <response code="200">Returns the categorys list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetCategory()
        {

            var category = _categoryService.GetAll();
            var categoryModel = _mapper.Map<IList<CategoryModel>>(category);
            return Ok(categoryModel);

        }

        /// <summary>
        /// Retrieves a category by ID
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>A response with category</returns>
        /// <response code="200">Returns the categorys list</response>
        /// <response code="404">If category is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Category/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategory([FromRoute] long id)
        {

            var response = new SingleResponse<CategoryModel>();
            try
            {
                var category = _categoryService.GetById(id);
                var categoryModel = _mapper.Map<CategoryModel>(category);
                response.Model = categoryModel;
            }
            catch
            {
                response.Error = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

            }
            return response.ToHttpResponse();
        }


    }
}
