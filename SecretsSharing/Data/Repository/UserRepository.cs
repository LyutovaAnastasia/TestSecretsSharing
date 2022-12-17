using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Data.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // exception
        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {

                if (id != Guid.Empty)
                {
                    return await _context.Users.FirstOrDefaultAsync(t => t.Id == id) ?? null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
