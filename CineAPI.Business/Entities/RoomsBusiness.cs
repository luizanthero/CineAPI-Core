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
    public class RoomsBusiness : IRepository<Room>, IViewModel<RoomDetailsViewModel>
    {
        private readonly AppDbContext context;

        public RoomsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Rooms.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Rooms.CountAsync(item => !item.IsActived);

        public async Task<Room> Create(Room entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Rooms.Add(entity);

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
            var room = await context.Rooms.FindAsync(id);

            if (room is null)
                return false;

            try
            {
                room.IsActived = false;
                context.Rooms.Update(room);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Room>> GetAll()
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .Where(item => item.IsActived).ToListAsync();

        public async Task<PaginationViewModel<Room>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<Room>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.Rooms
                    .Include(item => item.RoomType).Include(item => item.Screen)
                    .Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<Room> GetById(int id)
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Room>> GetByScreen(int id)
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .Where(item => item.IsActived && item.ScreenId == id).ToListAsync();

        public async Task<IEnumerable<Room>> GetByRoomType(int id)
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .Where(item => item.IsActived && item.RoomTypeId == id).ToListAsync();

        public async Task<IEnumerable<Room>> GetByName(string name)
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .Where(item => item.IsActived && item.Name.Contains(name)).ToListAsync();

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Rooms
                .Include(item => item.RoomType).Include(item => item.Screen)
                .Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Name }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Rooms.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Room entity)
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

        public async Task<IEnumerable<RoomDetailsViewModel>> GetAllDetails()
        {
            List<RoomDetailsViewModel> result = await (from room in context.Rooms
                                                        .Include(item => item.Screen).Include(item => item.RoomType)
                                                       let exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                           .Include(item => item.Film).Include(item => item.Room)
                                                           .Include(item => item.Schedule)
                                                           .Where(item => item.RoomId == room.id)
                                                           .Select(item => new ExhibitionDetailsViewModel()
                                                           {
                                                               id = item.id,
                                                               Filme = item.Film.Name,
                                                               ApiCode = item.Film.ApiCode,
                                                               Room = item.Room.Name,
                                                               Schedule = item.Schedule.Description,
                                                               created_at = item.created_at,
                                                               updated_at = item.updated_at
                                                           })
                                                       where room.IsActived
                                                       select new RoomDetailsViewModel()
                                                       {
                                                           id = room.id,
                                                           Name = room.Name,
                                                           RoomType = room.RoomType.Description,
                                                           Screen = room.Screen.Size,
                                                           Exhibitions = exhibitions,
                                                           created_at = room.created_at,
                                                           updated_at = room.updated_at
                                                       }).ToListAsync();

            return result;
        }

        public async Task<RoomDetailsViewModel> GetDetails(int id)
        {
            RoomDetailsViewModel result = await (from room in context.Rooms
                                                    .Include(item => item.Screen).Include(item => item.RoomType)
                                                 let exhibitions = (ICollection<ExhibitionDetailsViewModel>)context.Exhibitions
                                                     .Include(item => item.Film).Include(item => item.Room)
                                                     .Include(item => item.Schedule)
                                                     .Where(item => item.RoomId == room.id)
                                                     .Select(item => new ExhibitionDetailsViewModel()
                                                     {
                                                         id = item.id,
                                                         Filme = item.Film.Name,
                                                         ApiCode = item.Film.ApiCode,
                                                         Room = item.Room.Name,
                                                         Schedule = item.Schedule.Description,
                                                         created_at = item.created_at,
                                                         updated_at = item.updated_at
                                                     })
                                                 where room.IsActived && room.id == id
                                                 select new RoomDetailsViewModel()
                                                 {
                                                     id = room.id,
                                                     Name = room.Name,
                                                     RoomType = room.RoomType.Description,
                                                     Screen = room.Screen.Size,
                                                     Exhibitions = exhibitions,
                                                     created_at = room.created_at,
                                                     updated_at = room.updated_at
                                                 }).FirstOrDefaultAsync();

            return result;
        }
    }
}
