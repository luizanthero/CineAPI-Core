using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActived { get; set; } = true;

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}