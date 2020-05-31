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
    public class RolesBusiness : IRepository<Role>
    {
        private readonly AppDbContext context;

        public RolesBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Roles.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Roles.CountAsync(item => !item.IsActived);

        public async Task<Role> Create(Role entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Roles.Add(entity);

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
            Role role = await context.Roles.FindAsync(id);

            if (role is null)
                return false;

            try
            {
                role.IsActived = false;
                context.Roles.Update(role);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAll()
            => await context.Roles.Where(item => item.IsActived).ToListAsync();

        public async Task<IEnumerable<Role>> GetAllPaginate(int page, int limitPage)
            => await context.Roles.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Role> GetById(int id)
            => await context.Roles.FindAsync(id);

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Roles.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel { id = item.id, Value = item.Description }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Roles.AnyAsync(item => item.id.Equals(id));

        public async Task<bool> Update(Role entity)
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