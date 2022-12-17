using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsSharing.Data.Repository.impl
{
    public class TextFileRepository : IFileRepository<TextFile>
    {
        private readonly AppDbContext _context;

        public TextFileRepository(AppDbContext context)
        {
            _context = context;
        }

        // exception
        public async Task<TextFile> GetByIdAsync(Guid id)
        {
            try
            {
                
                if (id != Guid.Empty)
                {
                    return await _context.TextFiles.FirstOrDefaultAsync(t => t.Id == id) ?? null;
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

        // exception
        public async Task<IEnumerable<TextFile>> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                if (userId != Guid.Empty)
                {
                    return await _context.TextFiles.Where(t => t.UserId == userId).ToListAsync() ?? null;
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

        // exception
        public async Task CreateAsync(TextFile textFile)
        {
            try
            {
                if (textFile != null)
                {
                    _context.Entry(textFile).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // exception
        public async Task DeleteAsync(TextFile textFile)
        {
            try
            {
                if (textFile != null)
                {
                    _context.Entry(textFile).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
