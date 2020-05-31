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
    public class HistoricsBusiness : IRepository<Historic>
    {
        private readonly AppDbContext context;

        public HistoricsBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountActived()
            => await context.Historics.CountAsync();

        public Task<int> CountDesactived()
            => throw new System.NotImplementedException();

        public async Task<Historic> Create(Historic entity)
        {
            try
            {
                context.Historics.Add(entity);

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
            Historic historic = await context.Historics.FindAsync(id);

            if (historic is null)
                return false;

            try
            {
                context.Historics.Remove(historic);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Historic>> GetAll()
            => await context.Historics.ToListAsync();

        public async Task<IEnumerable<Historic>> GetAllPaginate(int page, int limitPage)
            => await context.Historics.Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<Historic> GetById(int id)
            => await context.Historics.FindAsync(id);

        public Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
            => throw new System.NotImplementedException();

        public async Task<bool> IsExist(int id)
            => await context.Historics.AnyAsync(item => item.id.Equals(id));

        public Task<bool> Update(Historic entity)
            => throw new System.NotImplementedException();
    }
}