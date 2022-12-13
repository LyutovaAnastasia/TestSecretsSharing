using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretsSharing.Data.Repository
{
    public class TextFileRepository
    {
        private readonly AppDbContext context;

        public TextFileRepository(AppDbContext context)
        {
            this.context = context;
        }

        public TextFile GetTextFile(Guid id)
        {
            return context.TextFiles.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<TextFile> GetTextFileAllByUserId(Guid userId)
        {
            return context.TextFiles.ToList();
        }

        public void CreateTextFile(TextFile textFile)
        {
            context.Entry(textFile).State = EntityState.Added;
            context.SaveChanges();
        }

        //public void DeleteTextFile(TextFile textFile)
        //{
        //    context.Entry(textFile).State = EntityState.Added;
        //    context.SaveChanges();
        //}

    }
}
