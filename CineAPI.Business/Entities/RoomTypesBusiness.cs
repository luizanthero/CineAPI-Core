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
    public class RoomTypesBusiness : IRepository<RoomType>, IViewModel<RoomTypeDetailsViewModel>
    {
        private readonly AppDbContext context;

        public RoomTypesBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<RoomType> Create(RoomType entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.RoomTypes.Add(entity);

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
            RoomType roomType = await context.RoomTypes.FindAsync(id);

            if (roomType is null)
                return false;

            try
            {
                roomType.IsActived = false;
                context.RoomTypes.Update(roomType);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoomType>> GetAll()
            => await context.RoomTypes.Where(item => item.IsActived).ToListAsync();

        public async Task<PaginationViewModel<RoomType>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<RoomType>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.RoomTypes.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<RoomType> GetById(int id)
            => await context.RoomTypes.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<RoomType>> GetByDescription(string description)
            => await context.RoomTypes.Where(item => item.IsActived && item.Description.Contains(description)).ToListAsync();

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.RoomTypes.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Description }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.RoomTypes.AnyAsync(item => item.id == id);

        public async Task<bool> Update(RoomType entity)
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
            => await context.RoomTypes.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.RoomTypes.CountAsync(item => !item.IsActived);

        public async Task<IEnumerable<RoomTypeDetailsViewModel>> GetAllDetails()
        {
            List<RoomTypeDetailsViewModel> result = await (from roomType in context.RoomTypes
                                                           let rooms = (ICollection<RoomDetailsViewModel>)context.Rooms
                                                               .Include(item => item.Screen).Include(item => item.RoomType)
                                                               .Where(item => item.RoomTypeId == roomType.id)
                                                               .Select(item => new RoomDetailsViewModel()
                                                               {
                                                                   id = item.id,
                                                                   Name = item.Name,
                                                                   RoomType = item.RoomType.Description,
                                                                   Screen = item.Screen.Size,
                                                                   Exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                                       .Include(x => x.Film).Include(x => x.Room).Include(x => x.Schedule)
                                                                       .Where(x => x.RoomId == item.id)
                                                                       .Select(x => new ExhibitionDetailsViewModel()
                                                                       {
                                                                           id = x.id,
                                                                           Filme = x.Film.Name,
                                                                           ApiCode = x.Film.ApiCode,
                                                                           Room = x.Room.Name,
                                                                           Schedule = x.Schedule.Description,
                                                                           created_at = x.created_at,
                                                                           updated_at = x.updated_at
                                                                       }),
                                                                   created_at = item.created_at,
                                                                   updated_at = item.updated_at
                                                               })
                                                           where roomType.IsActived
                                                           select new RoomTypeDetailsViewModel()
                                                           {
                                                               id = roomType.id,
                                                               Description = roomType.Description,
                                                               Rooms = rooms,
                                                               created_at = roomType.created_at,
                                                               updated_at = roomType.updated_at
                                                           }).ToListAsync();

            return result;
        }

        public async Task<RoomTypeDetailsViewModel> GetDetails(int id)
        {
            RoomTypeDetailsViewModel result = await (from roomType in context.RoomTypes
                                                     let rooms = (ICollection<RoomDetailsViewModel>)context.Rooms
                                                         .Include(item => item.Screen).Include(item => item.RoomType)
                                                         .Where(item => item.RoomTypeId == roomType.id)
                                                         .Select(item => new RoomDetailsViewModel()
                                                         {
                                                             id = item.id,
                                                             Name = item.Name,
                                                             RoomType = item.RoomType.Description,
                                                             Screen = item.Screen.Size,
                                                             Exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                                 .Include(x => x.Film).Include(x => x.Room).Include(x => x.Schedule)
                                                                 .Where(x => x.RoomId == item.id)
                                                                 .Select(x => new ExhibitionDetailsViewModel()
                                                                 {
                                                                     id = x.id,
                                                                     Filme = x.Film.Name,
                                                                     ApiCode = x.Film.ApiCode,
                                                                     Room = x.Room.Name,
                                                                     Schedule = x.Schedule.Description,
                                                                     created_at = x.created_at,
                                                                     updated_at = x.updated_at
                                                                 }),
                                                             created_at = item.created_at,
                                                             updated_at = item.updated_at
                                                         })
                                                     where roomType.IsActived
                                                     select new RoomTypeDetailsViewModel()
                                                     {
                                                         id = roomType.id,
                                                         Description = roomType.Description,
                                                         Rooms = rooms,
                                                         created_at = roomType.created_at,
                                                         updated_at = roomType.updated_at
                                                     }).FirstOrDefaultAsync();

            return result;
        }
    }
}
