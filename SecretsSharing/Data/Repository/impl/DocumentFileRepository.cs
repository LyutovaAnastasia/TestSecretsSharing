using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsSharing.Data.Repository.impl
{
    public class DocumentFileRepository : IFileRepository<DocumentFile>
    {
        private readonly AppDbContext _context;

        public DocumentFileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentFile> GetByIdAsync(Guid id)
        {
            return await _context.DocumentFiles.FirstOrDefaultAsync(t => t.Id == id);
        }
        
        public async Task<IEnumerable<DocumentFile>> GetAllByUserIdAsync(int userId)
        {
            try
            {
                return await _context.DocumentFiles.Where(d => d.UserId == userId).ToListAsync() ?? null;
            }
            catch
            {
                throw new Exception("Failed to get document files.");
            }

        }

        public async Task CreateAsync(DocumentFile documentFile)
        {
            try
            {
                _context.Entry(documentFile).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to create document files.");
            }
        }

        public async Task DeleteAsync(DocumentFile documentFile)
        {
            try
            {
                _context.Entry(documentFile).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Failed to delete document files.");
            }
        }
    }
}
