using CineAPI.Business.Interfaces;
using CineAPI.Models;
using CineAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineAPI.Business.Entities
{
    public class UserRolesBusiness : IRepository<UserRole>
    {
        private readonly AppDbContext context;

        public UserRolesBusiness(AppDbContext context)
        {
            this.context = context;
        }

        public Task<int> CountActived()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountDesactived()
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserRole> Create(UserRole entity)
        {
            entity.created_at = DateTime.Now;

            try
            {
                context.UserRoles.Add(entity);

                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UserRole>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginationViewModel<UserRole>> GetAllPaginate(int page, int limitPage)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserRole> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ComboBoxViewModel>> GetComboBox()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(UserRole entity)
        {
            throw new System.NotImplementedException();
        }
    }
}