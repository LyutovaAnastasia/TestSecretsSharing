using System;

namespace SecretsSharing.DTO
{
    /// <summary>
    /// Class DTO for text file.
    /// </summary>
    public class TextFileDTO : TextFileRequestDTO
    {
        public DateTime? AddDate { get; set; }
    }
}
