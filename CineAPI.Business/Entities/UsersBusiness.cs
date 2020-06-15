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
        private readonly RolesBusiness rolesBusiness;

        private readonly SettingsOptions settingsOptions;

        public UsersBusiness(AppDbContext context, TokensBusiness tokensBusiness, RolesBusiness rolesBusiness,
        IOptions<SettingsOptions> settingsOptions)
        {
            this.context = context;
            this.tokensBusiness = tokensBusiness;
            this.rolesBusiness = rolesBusiness;
            this.settingsOptions = settingsOptions.Value;
        }

        public async Task<User> Register(User user)
        {
            try
            {
                user.Password = HashOptions.CreatePasswordHash(user.Password);

                context.Users.Add(user);

                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> Authenticate(string username, string password)
        {
            try
            {
                User user = await context.Users
                    .SingleOrDefaultAsync(item => item.Username.Equals(username));

                if (user is null)
                    return null;

                if (!HashOptions.VerifyPasswordHash(password, user.Password))
                    return null;

                var userRoles = await rolesBusiness.GetByUserId(user.id);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.id.ToString()));
                if (!(userRoles is null))
                    userRoles.ToList().ForEach(role =>
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Description));
                    });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(settingsOptions.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var stringToken = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(stringToken);
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
            => await context.Users.Include(item => item.UserRoles).Where(item => item.IsActived).ToListAsync();

        public async Task<PaginationViewModel<User>> GetAllPaginate(int page, int limitPage)
            => new PaginationViewModel<User>
            {
                Page = page,
                LimitPage = limitPage,
                TotalPages = await CountActived(),
                Data = await context.Users.Include(item => item.UserRoles)
                    .Where(item => item.IsActived).Skip((page - 1) * limitPage).Take(limitPage).ToListAsync()
            };

        public async Task<User> GetById(int id)
            => await context.Users.Include(item => item.UserRoles).FirstOrDefaultAsync(item => item.id == id);

        public async Task<User> GetByUsername(string username)
            => await context.Users.Include(item => item.UserRoles).FirstOrDefaultAsync(item => item.Username.Equals(username));

        public async Task<User> GetByEmail(string email)
            => await context.Users.Include(item => item.UserRoles).FirstOrDefaultAsync(item => item.Email.Equals(email));

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