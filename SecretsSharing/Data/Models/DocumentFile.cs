using System;

namespace SecretsSharing.Data.Models
{
    public class DocumentFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime AddDate { get; set; }
    }
}
