using HeroesAPI.Interfaces;
using HeroesAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HeroesAPI.Services
{
    public class HeroService : IHeroService
    {
        private readonly IMongoRepository<Hero> _heroesRepository;
        public HeroService(IMongoRepository<Hero> heroesRepository)
        {
            _heroesRepository = heroesRepository;
        }
        public async Task<IEnumerable<Hero>> GetAllHeroesAsync() =>
            _heroesRepository.GetAsync();

        public ActionResult<Hero> GetHeroById(string id)
        {
            var hero = _heroesRepository.FindById(id);
            if (hero == null)
            {
                return null;
            }
            return hero;
        }

        public IEnumerable<Hero> SearchHeroByTerm(string term)
        {
            return _heroesRepository.FilterBy(h => h.HeroName.Contains(term));
        }
        public async Task GenExcelFile()
        {
            var heroes = _heroesRepository.GetAsync();

            await Excell.Excel(heroes);
        }

        public async Task<Hero> CreateNewHero(string heroName)
        {
            var newHero = new Hero() { HeroName = heroName };

            await _heroesRepository.InsertOneAsync(newHero);

            return newHero;
        }
        public async Task<IActionResult> UpdateHeroAsync(string id, Hero updatehero)
        {
            var hero = await _heroesRepository.FindByIdAsync(id);

            if (hero is null)
            {
                return null;
            }
            updatehero.Id = hero.Id;

            await _heroesRepository.ReplaceOneAsync(updatehero);

            return null;

        }
        public async Task<IActionResult> GenerateRandomHero(int quantity)
        {
            await _heroesRepository.GenerateRandomAsync(quantity);
            return null;

        }

        public async Task DeleteHero(string id)
        {
            var hero = await _heroesRepository.FindByIdAsync(id);

            await _heroesRepository.DeleteByIdAsync(hero.Id);


        }
    }
}
