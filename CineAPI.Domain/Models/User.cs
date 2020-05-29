using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table(name: "User")]
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsActived { get; set; } = true;

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}