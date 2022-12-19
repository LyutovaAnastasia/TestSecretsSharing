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

        public async Task<TextFile> GetByIdAsync(Guid id)
        {
            return await _context.TextFiles.FirstOrDefaultAsync(t => t.Id == id);
        }

        // exception
        public async Task<IEnumerable<TextFile>> GetAllByUserIdAsync(int userId)
        {
            try
            {
                return await _context.TextFiles.Where(t => t.UserId == userId).ToListAsync() ?? null;
            }
            catch
            {
                throw new Exception("Failed to get text files.");
            }

        }

        public async Task CreateAsync(TextFile textFile)
        {
            try
            {
                _context.Entry(textFile).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to create text file.");
            }
        }

        public async Task DeleteAsync(TextFile textFile)
        {
            try
            {
                _context.Entry(textFile).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to delete text file.");
            }

        }
    }
}
