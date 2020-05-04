using CineAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CineAPI.Business.Entities
{
    public class FilmsBusiness
    {
        private AppDbContext context;

        public FilmsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Film>> GetFilms()
            => await context.Films.ToListAsync();
    }
}
