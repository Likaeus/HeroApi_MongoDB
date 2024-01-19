using HeroesAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HeroesAPI.Models;
using Microsoft.AspNetCore.Http;
using HeroesAPI.Services;
using System.Net;
using static MongoDB.Libmongocrypt.CryptContext;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(HeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHeroes()
        {
            try
            {
                var heroList = await _heroService.GetAllHeroesAsync();

                var statusCode = heroList.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NoContent;

                return StatusCode(statusCode, heroList);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error al obtener la lista de héroes: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public ActionResult<Hero> GetById(string id)
        {
            try
            {
                var hero = _heroService.GetHeroById(id);

                if (hero == null)
                {
                    return NotFound();
                }

                return Ok(hero);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error al obtener el héroe: {ex.Message}");
            }
        }

        [HttpGet("search/{term}")]
        public async Task<IActionResult> SearchByTerm(string term)
        {
            try
             {
                var heroesTask = _heroService.GetAllHeroesAsync();
                var heroes = await heroesTask;
                var matchingHeroes = heroes
                    .Where(hero => hero.HeroName.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!matchingHeroes.Any())
                {
                    return NotFound($"No se encontraron héroes que coincidan con el término '{term}'.");
                }

                return Ok(matchingHeroes);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error al realizar la búsqueda de héroes: {ex.Message}");
            }
        }
        [HttpGet("ExcelGen")]
        public async Task GenExcel()
        {
           await  _heroService.GenExcelFile();

        }

        [HttpPost]
        public async Task<IActionResult> CreateHero(string heroName)
        {
            try
            {
                var newHero = new Hero() { HeroName = heroName };

                await _heroService.CreateNewHero(newHero.HeroName);

                
                return CreatedAtAction(nameof(GetById), new { id = newHero.Id }, newHero);
            }
            catch (Exception ex)
            {
                
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error al crear el héroe: {ex.Message}");
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateHero(string id, Hero updatehero)
        {
            try
            {
                await _heroService.UpdateHeroAsync(id, updatehero);
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error al actualizar el héroe: {ex.Message}");
            }

        }
        [HttpPost("generate/{quantity}")]
        public async Task<IActionResult>GenRandHero(int quantity)
        {
            await _heroService.GenerateRandomHero(quantity);


            return NoContent(); 
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var hero = _heroService.GetHeroById(id);

            if(hero is null)
            {
                return NotFound(hero);
            }
            await _heroService.DeleteHero(id);
            return NoContent();
        }
        
    }
}
