using System;
using System.ComponentModel.DataAnnotations;

namespace SecretsSharing.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email {get; set;}
        public string Password { get; set; }
        public bool AutoDelete { get; set; }
    }
}
