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
    public class HistoricTypesBusiness : IRepository<HistoricType>
    {
        private readonly AppDbContext context;

        public HistoricTypesBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.HistoricTypes.CountAsync(item => item.IsActived);

        public async Task<int> CountDesactived()
            => await context.HistoricTypes.CountAsync(item => !item.IsActived);

        public async Task<HistoricType> Create(HistoricType entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.HistoricTypes.Add(entity);

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
            HistoricType historicType = await context.HistoricTypes.FindAsync(id);

            if (historicType is null)
                return false;

            try
            {
                historicType.IsActived = false;
                context.HistoricTypes.Update(historicType);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HistoricType>> GetAll()
            => await context.HistoricTypes.Where(item => item.IsActived).ToListAsync();

        public async Task<IEnumerable<HistoricType>> GetAllPaginate(int page, int limitPage)
            => await context.HistoricTypes.Where(item => item.IsActived)
                .Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<HistoricType> GetById(int id)
            => await context.HistoricTypes.FindAsync(id);

        public async Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => await context.HistoricTypes.Where(item => item.IsActived)
                .Select(item => new ComboBoxViewModel { id = item.id, Value = item.Description }).ToListAsync();

        public async Task<bool> IsExist(int id)
            => await context.HistoricTypes.AnyAsync(item => item.id.Equals(id));

        public async Task<bool> Update(HistoricType entity)
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