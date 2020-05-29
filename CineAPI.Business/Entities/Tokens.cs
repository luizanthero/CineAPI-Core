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
    public class TokensBusiness : IRepository<Tokens>
    {
        private readonly AppDbContext context;

        public TokensBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Tokens.CountAsync();

        public Task<int> CountDesactived()
            => throw new System.NotImplementedException();

        public async Task<Tokens> Create(Tokens entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Tokens.Add(entity);

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
            Tokens token = await context.Tokens.FindAsync(id);

            if (token is null)
                return false;

            try
            {
                context.Tokens.Remove(token);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
            Tokens token = await context.Tokens.FirstOrDefaultAsync(item => item.UserId == userId);

            if (token is null)
                return false;

            try
            {
                context.Tokens.Remove(token);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tokens>> GetAll()
            => await context.Tokens.Include(item => item.User).ToListAsync();

        public async Task<IEnumerable<Tokens>> GetAllPaginate(int page, int limitPage)
            => await context.Tokens.Include(item => item.User).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Tokens> GetById(int id)
            => await context.Tokens.Include(item => item.User).FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Tokens
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Token }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Tokens.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Tokens entity)
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