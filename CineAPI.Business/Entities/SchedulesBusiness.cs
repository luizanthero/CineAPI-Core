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
    public class SchedulesBusiness : IRepository<Schedule>
    {
        private readonly AppDbContext context;

        public SchedulesBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Schedules.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.Schedules.CountAsync(item => !item.IsActived);

        public async Task<Schedule> Create(Schedule entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.Schedules.Add(entity);

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
            Schedule schedule = await context.Schedules.FindAsync(id);

            if (schedule is null)
                return false;

            try
            {
                schedule.IsActived = false;
                context.Schedules.Update(schedule);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetAll()
            => await context.Schedules.Where(item => item.IsActived).ToListAsync();

        public async Task<PaginationViewModel<Schedule>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<Schedule>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.Schedules.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<Schedule> GetById(int id)
            => await context.Schedules.FirstOrDefaultAsync(item => item.id == id);

        public async Task<IEnumerable<Schedule>> GetByDescription(string description)
            => await context.Schedules.Where(item => item.IsActived && item.Description.Contains(description)).ToListAsync();

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.Schedules.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel() { id = item.id, Value = item.Description }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.Schedules.AnyAsync(item => item.id == id);

        public async Task<bool> Update(Schedule entity)
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
