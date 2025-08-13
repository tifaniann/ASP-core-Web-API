using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }                // Primary key
        public string Username { get; set; }       // Username login
        public string PasswordHash { get; set; }   // Password (hash)
        public string Role { get; set; } 
    }
}