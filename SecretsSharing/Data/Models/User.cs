
namespace SecretsSharing.Data.Models
{
    /// <summary>
    /// Class model user.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Email {get; set;}
        public string PasswordSalt { get; set;}
        public string PasswordHash { get; set; }
        public bool AutoDeleteText { get; set; }
        public bool AutoDeleteDocument { get; set; }
    }
}
