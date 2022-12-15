using System;
using System.ComponentModel.DataAnnotations;

namespace SecretsSharing.Data.Models
{
    public class TextFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime AddDate { get; set; }
    }
}
