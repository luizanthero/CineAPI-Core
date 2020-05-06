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
    public class RoomsBusiness : IRepository<Room>
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
            => await context.Rooms.Where(item => item.IsActived).ToListAsync();

        public async Task<IEnumerable<Room>> GetAllPaginate(int page, int limitPage)
            => await context.Rooms.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Room> GetById(int id)
            => await context.Rooms.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Room>> GetByScreen(int id)
            => await context.Rooms.Where(item => item.IsActived && item.ScreenId == id).ToListAsync();

        public async Task<IEnumerable<Room>> GetByRoomType(int id)
            => await context.Rooms.Where(item => item.IsActived && item.RoomTypeId == id).ToListAsync();

        public async Task<IEnumerable<Room>> GetByName(string name)
            => await context.Rooms.Where(item => item.IsActived && item.Name.Contains(name)).ToListAsync();

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Rooms.Where(item => item.IsActived)
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
    }
}
