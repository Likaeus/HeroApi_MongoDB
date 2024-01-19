using HeroesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeroesAPI.Interfaces
{
    public interface IHeroService
    {
        Task<Hero> CreateNewHero(string heroName);
        Task DeleteHero(string id);
        Task<IActionResult> GenerateRandomHero(int quantity);
        Task GenExcelFile();
        Task<IEnumerable<Hero>> GetAllHeroesAsync();
        ActionResult<Hero> GetHeroById(string id);
        IEnumerable<Hero> SearchHeroByTerm(string term);
        Task<IActionResult> UpdateHeroAsync(string id, Hero updatehero);
    }
}