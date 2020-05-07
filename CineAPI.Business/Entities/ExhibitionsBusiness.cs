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
    public class ExhibitionsBusiness : IRepository<Exhibition>, IViewModel<ExhibitionDetailsViewModel>
    {
        private readonly AppDbContext context;

        public ExhibitionsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Exhibitions.CountAsync();

        public Task<int> CountDesactived()
            => throw new NotImplementedException();

        public async Task<Exhibition> Create(Exhibition entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Exhibitions.Add(entity);

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
            Exhibition exhibition = await context.Exhibitions.FindAsync(id);

            if (exhibition is null)
                return false;

            try
            {
                context.Exhibitions.Remove(exhibition);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByFilm(int id)
        {
            List<Exhibition> exhibitions = await context.Exhibitions.Where(item => item.FilmId == id).ToListAsync();

            if (exhibitions is null)
                return false;

            try
            {
                context.Exhibitions.RemoveRange(exhibitions);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByRoom(int id)
        {
            List<Exhibition> exhibitions = await context.Exhibitions.Where(item => item.RoomId == id).ToListAsync();

            if (exhibitions is null)
                return false;

            try
            {
                context.Exhibitions.RemoveRange(exhibitions);

                await context.SaveChangesAsync();

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteBySchedule(int id)
        {
            List<Exhibition> exhibitions = await context.Exhibitions.Where(item => item.ScheduleId == id).ToListAsync();

            if (exhibitions is null)
                return false;

            try
            {
                context.Exhibitions.RemoveRange(exhibitions);

                await context.SaveChangesAsync();

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Exhibition>> GetAll()
        {
            try
            {
                List<Exhibition> result = await context.Exhibitions
                    .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule).ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Exhibition>> GetAllPaginate(int page, int limitPage)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Exhibition> GetById(int id)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Exhibition>> GetByFilm(int id)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Where(item => item.FilmId == id).ToListAsync();

        public async Task<IEnumerable<Exhibition>> GetByRoom(int id)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Where(item => item.RoomId == id).ToListAsync();

        public async Task<IEnumerable<Exhibition>> GetBySchedule(int id)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Where(item => item.ScheduleId == id).ToListAsync();

        public Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => throw new NotImplementedException();

        public async Task<bool> IsExist(int id)
            => await context.Exhibitions.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Exhibition entity)
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

        public async Task<IEnumerable<ExhibitionDetailsViewModel>> GetAllDetails()
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Where(item => item.Film.IsActived && item.Room.IsActived && item.Schedule.IsActived)
                .Select(item => new ExhibitionDetailsViewModel()
                {
                    id = item.id,
                    Filme = item.Film.Name,
                    ApiCode = item.Film.ApiCode,
                    Room = item.Room.Name,
                    Schedule = item.Schedule.Description,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                }).ToListAsync();

        public async Task<ExhibitionDetailsViewModel> GetDetails(int id)
            => await context.Exhibitions
                .Include(item => item.Film).Include(item => item.Room).Include(item => item.Schedule)
                .Select(item => new ExhibitionDetailsViewModel()
                {
                    id = item.id,
                    Filme = item.Film.Name,
                    ApiCode = item.Film.ApiCode,
                    Room = item.Room.Name,
                    Schedule = item.Schedule.Description,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                }).FirstOrDefaultAsync(item => item.id == id);
    }
}
