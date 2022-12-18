
using System;
using System.Text;

namespace SecretsSharing.Data.Models
{
    /// <summary>
    /// Base class for text and document files.
    /// </summary>
    public abstract class Content
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public string GetUrl(string sufix, Guid id)
        {
            return sufix + Convert
                          .ToBase64String(Encoding.UTF8.GetBytes(Convert
                           .ToString(id)));
        }
        
    }
}
