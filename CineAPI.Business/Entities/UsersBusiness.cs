using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineAPI.Business.Interfaces;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Business.Entities
{
    public class UsersBusiness : IRepository<User>
    {
        private readonly AppDbContext context;

        public UsersBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Users.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Users.CountAsync(item => !item.IsActived);

        public async Task<User> Create(User entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Users.Add(entity);

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
            User user = await context.Users.FindAsync(id);

            if (user is null)
                return false;

            try
            {
                user.IsActived = false;
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
            => await context.Users.Where(item => item.IsActived).ToListAsync();

        public async Task<IEnumerable<User>> GetAllPaginate(int page, int limitPage)
            => await context.Users.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<User> GetById(int id)
            => await context.Users.FirstOrDefaultAsync(item => item.id == id);

        public async Task<User> GetByUsername(string username)
            => await context.Users.FirstOrDefaultAsync(item => item.Username.Equals(username));

        public async Task<User> GetByEmail(string email)
            => await context.Users.FirstOrDefaultAsync(item => item.Email.Equals(email));

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Users.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Username }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Users.AnyAsync(item => item.id == id);

        public async Task<bool> Update(User entity)
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