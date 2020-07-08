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
    public class FilmsBusiness : IRepository<Film>, IViewModel<FilmDetailsViewModel>
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

        public async Task<PaginationViewModel<Film>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<Film>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.Films.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<Film> GetById(int id)
            => await context.Films.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Film>> GetByName(string name)
            => await context.Films.Where(item => item.IsActived && item.Name.Contains(name)).ToListAsync();

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

        public async Task<int> CountActived()
            => await context.Films.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Films.CountAsync(item => !item.IsActived);

        public async Task<IEnumerable<FilmDetailsViewModel>> GetAllDetails()
        {
            List<FilmDetailsViewModel> result = await (from film in context.Films
                                                       let exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                           .Include(item => item.Room).Include(item => item.Schedule)
                                                           .Where(item => item.FilmId == film.id)
                                                           .Select(item => new ExhibitionDetailsViewModel()
                                                           {
                                                               id = item.id,
                                                               Film = item.Film.Name,
                                                               Poster = item.Film.Poster,
                                                               Year = item.Film.Year,
                                                               ApiCode = item.Film.ApiCode,
                                                               Room = item.Room.Name,
                                                               Schedule = item.Schedule.Description,
                                                               created_at = item.created_at,
                                                               updated_at = item.updated_at
                                                           })
                                                       where film.IsActived
                                                       select new FilmDetailsViewModel()
                                                       {
                                                           id = film.id,
                                                           Name = film.Name,
                                                           ApiCode = film.ApiCode,
                                                           Exhibitions = exhibitions
                                                       }).ToListAsync();

            return result;
        }

        public async Task<FilmDetailsViewModel> GetDetails(int id)
        {
            FilmDetailsViewModel result = await (from film in context.Films
                                                 let exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                     .Include(item => item.Room).Include(item => item.Schedule)
                                                     .Where(item => item.id == film.id)
                                                     .Select(item => new ExhibitionDetailsViewModel()
                                                     {
                                                         id = item.id,
                                                         Film = item.Film.Name,
                                                         Poster = item.Film.Poster,
                                                         Year = item.Film.Year,
                                                         ApiCode = item.Film.ApiCode,
                                                         Room = item.Room.Name,
                                                         Schedule = item.Schedule.Description,
                                                         created_at = item.created_at,
                                                         updated_at = item.updated_at
                                                     })
                                                 where film.IsActived && film.id == id
                                                 select new FilmDetailsViewModel()
                                                 {
                                                     id = film.id,
                                                     Name = film.Name,
                                                     ApiCode = film.ApiCode,
                                                     Plot = film.Plot,
                                                     Actors = film.Actors,
                                                     Awards = film.Awards,
                                                     Director = film.Director,
                                                     Poster = film.Poster,
                                                     Production = film.Production,
                                                     Runtime = film.Runtime,
                                                     Website = film.Website,
                                                     Writer = film.Writer,
                                                     Year = film.Year,
                                                     Genre = film.Genre,
                                                     Exhibitions = exhibitions
                                                 }).FirstOrDefaultAsync();

            return result;
        }
    }
}
