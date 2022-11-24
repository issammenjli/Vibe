using Microsoft.AspNetCore.Mvc;
using Vibe.Entities;
using Vibe.WebApi.Data;

namespace Vibe.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SingerController : ControllerBase
    {
        private readonly ISingerRepository SingerRepository;

        public SingerController(ISingerRepository SingerRepository)
        {
            this.SingerRepository = SingerRepository;
        }

        #region methodes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Singers = await SingerRepository.GetAll();
            if (Singers.Count() == 0)
                return NoContent();

            return Ok(Singers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var Singer = await SingerRepository.GetOne(id);

            if (Singer == null)
                return NotFound();

            return Ok(Singer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Singer Singer)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            await SingerRepository.Add(Singer);
            return CreatedAtAction(nameof(GetOne), new { id = Singer.Id }, Singer);
        }
        #endregion
    }
}