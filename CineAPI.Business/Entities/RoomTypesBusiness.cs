﻿using CineAPI.Business.Interfaces;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineAPI.Business.Entities
{
    public class RoomTypesBusiness : IRepository<RoomType>
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

        public async Task<IEnumerable<RoomType>> GetAllPaginate(int page, int limitPage)
            => await context.RoomTypes.Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync();

        public async Task<RoomType> GetById(int id)
            => await context.RoomTypes.FirstOrDefaultAsync(item => item.IsActived && item.id == id);

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
    }
}