using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CineAPI.Business.Helpers;
using CineAPI.Business.Interfaces;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CineAPI.Business.Entities
{
    public class UsersBusiness : IRepository<User>
    {
        private readonly AppDbContext context;
        private readonly TokensBusiness tokensBusiness;

        private readonly Settings settings;

        public UsersBusiness(AppDbContext context, TokensBusiness tokensBusiness, IOptions<Settings> settings)
        {
            this.context = context;
            this.tokensBusiness = tokensBusiness;
            this.settings = settings.Value;
        }

        public async Task<Tokens> Authenticate(string username, string password)
        {
            try
            {
                User user = await context.Users
                    .SingleOrDefaultAsync(item => item.Username.Equals(username) && item.Password.Equals(password));

                if (user is null)
                    return null;

                Tokens token = await tokensBusiness.GetbyUserId(user.id);

                if (!(token is null))
                    return token;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(settings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var stringToken = tokenHandler.CreateToken(tokenDescriptor);
                token = await tokensBusiness.Create(new Tokens
                {
                    UserId = user.id,
                    Token = tokenHandler.WriteToken(stringToken),
                    Type = "Jwt",
                    IsRevoked = true
                });

                return token;
            }
            catch (Exception)
            {
                throw;
            }
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