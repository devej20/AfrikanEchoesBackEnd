using AfrikanEchoes.Entities;
using AfrikanEchoes.Models.Languages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Route("api/Languages")]
    [ApiController]
    public class LanguagesApiController : ControllerBase
    {
        private readonly AfrikanEchoesDbContext _context;

        public LanguagesApiController(AfrikanEchoesDbContext context)
        {
            _context = context;
        }

        // GET: api/LanguagesApi
        [HttpGet]
        public IEnumerable<LanguageModel> GetLanguage()
        {
            return _context.Languages.Select(s =>
            new LanguageModel
            {
                Id = s.Id,
                Name = s.Name
            }
            );
        }

        // GET: api/LanguagesApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLanguage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return NotFound();
            }

            return Ok(language);
        }

        // PUT: api/LanguagesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguage([FromRoute] long id, [FromBody] Language language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != language.Id)
            {
                return BadRequest();
            }

            _context.Entry(language).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LanguagesApi
        [HttpPost]
        public async Task<IActionResult> PostLanguage([FromBody] Language language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguage", new { id = language.Id }, language);
        }

        // DELETE: api/LanguagesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return Ok(language);
        }

        private bool LanguageExists(long id)
        {
            return _context.Languages.Any(e => e.Id == id);
        }
    }
}
