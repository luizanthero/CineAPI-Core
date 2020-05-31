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
    public class ScreensBusiness : IRepository<Screen>
    {
        private readonly AppDbContext context;

        public ScreensBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Screens.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Screens.CountAsync(item => !item.IsActived);

        public async Task<Screen> Create(Screen entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Screens.Add(entity);

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
            Screen screen = await context.Screens.FindAsync(id);

            if (screen is null)
                return false;

            try
            {
                screen.IsActived = false;
                context.Screens.Update(screen);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Screen>> GetAll()
            => await context.Screens.Where(item => item.IsActived).ToListAsync();

        public async Task<PaginationViewModel<Screen>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<Screen>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.Screens.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<Screen> GetById(int id)
            => await context.Screens.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Screen>> GetBySize(string size)
            => await context.Screens.Where(item => item.IsActived && item.Size.Contains(size)).ToListAsync();

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Screens.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Size }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Screens.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Screen entity)
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
