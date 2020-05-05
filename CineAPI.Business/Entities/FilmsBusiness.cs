using CineAPI.Business.Interfaces;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineAPI.Business.Entities
{
    public class FilmsBusiness : IRepository<Film>
    {
        private readonly AppDbContext context;

        public FilmsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Film> Create(Film entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Films.Add(entity);

                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            Film film = await context.Films.FindAsync(id);

            if (film is null)
                return false;

            try
            {
                film.IsActived = false;
                context.Films.Update(film);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Film>> GetAll()
            => await context.Films.Where(item => item.IsActived).ToListAsync();

        public async Task<IEnumerable<Film>> GetAllPaginate(int page, int limitPage)
            => await context.Films.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Film> GetById(int id)
            => await context.Films.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Film>> GetByName(string name)
            => await context.Films.Where(item => item.Name.Contains(name)).ToListAsync();

        public async Task<Film> GetByApiCode(string apiCode)
            => await context.Films.FirstOrDefaultAsync(item => item.ApiCode == apiCode);

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Films.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Name }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Films.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Film entity)
        {
            entity.updated_at = DateTime.Now;

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsExist(entity.id))
                    return false;

                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
