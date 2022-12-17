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

        // exception
        public async Task<DocumentFile> GetByIdAsync(Guid id)
        {
            try
            {
                
                if (id != Guid.Empty)
                {
                    return await _context.DocumentFiles.FirstOrDefaultAsync(t => t.Id == id) ?? null;
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
        public async Task<IEnumerable<DocumentFile>> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                if (userId != Guid.Empty)
                {
                    return await _context.DocumentFiles.Where(d => d.UserId == userId).ToListAsync() ?? null;
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
        public async Task CreateAsync(DocumentFile documentFile)
        {
            try
            {
                if (documentFile != null)
                { 
                    _context.Entry(documentFile).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // exception
        public async Task DeleteAsync(DocumentFile documentFile)
        {
            try
            {
                if (documentFile != null)
                {
                    _context.Entry(documentFile).State = EntityState.Deleted;
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
