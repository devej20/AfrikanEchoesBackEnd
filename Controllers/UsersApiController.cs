using AfrikanEchoes.Entities;
using AfrikanEchoes.Helpers;
using AfrikanEchoes.Services;
using AfrikanEchoes.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using AfricanEchoes.Models;

namespace AfrikanEchoes.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UsersApiController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersApiController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        // POST
        // api/UserApi/Authenticate/

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new user</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="201">A response as creation of user</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("Authenticate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {

            var response = new SingleResponse<User>();

            try
            {
                var user = _userService.Authenticate(model.PhoneNumber, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("Name", "Phone Number or Password is incorrect");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                user.Token = tokenString;

                // return basic user info (without password) and token to store client side        
                var UserModel = _mapper.Map<User>(user);
                response.Model = UserModel;
            }
            catch (Exception )
            {
                response.Error = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/UserApi/Register/

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new user</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="201">A response as creation of user</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            var response = new SingleResponse<User>();
            var user = _mapper.Map<User>(model);

            try
            {
                var userModel = await _userService.Create(user, model.Password);
                response.Model = userModel;
            }
            catch (AppException ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;

            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves user
        /// </summary>
        /// <returns>A response with users list</returns>
        /// <response code="200">Returns the users list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Users")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<User>>(users);
            return Ok(model);
        }

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>A response with user</returns>
        /// <response code="200">Returns the users list</response>
        /// <response code="404">If user is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("User/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<User>(user);
            return Ok(model);
        }

        // PUT
        // api/UserApi/Update/5

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as user result</returns>
        /// <response code="200">If user was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="404">If user is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE
        // api/UserApi/Delete/5

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="id">user ID</param>
        /// <returns>A response as delete user result</returns>
        /// <response code="200">If user was deleted successfully</response>
        /// <response code="404">If user is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
