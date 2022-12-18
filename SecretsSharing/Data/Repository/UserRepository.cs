using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System.Threading.Tasks;
using System;
using SecretsSharing.DTO;

namespace SecretsSharing.Data.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch
            {
                throw new Exception("User no found.");
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch
            {
                throw new Exception("User no found.");
            }
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to create user.");
            }
        }

        public async Task SetUserSettingsAsync(User user)
        {  
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to update user.");
            }
        }

    }
}
