using CineAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineAPI.Business.Entities
{
    public class FilmsBusiness
    {
        private readonly AppDbContext context;

        public FilmsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Film>> GetFilms()
            => await context.Films.ToListAsync();

        public async Task<Film> PostFilm(Film film)
        {
            context.Films.Add(film);

            await context.SaveChangesAsync();

            return film;
        }
    }
}
