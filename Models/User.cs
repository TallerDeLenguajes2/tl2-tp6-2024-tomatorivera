using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp6_2024_tomatorivera.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public AccessLevel AccessLevel { get; set; }
    }

    public enum AccessLevel
    {
        Admin,
        Usuario
    }
}