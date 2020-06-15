using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineAPI.Models
{
    [Table(name: "UserRole")]
    public class UserRole
    {
        [Required]
        [ForeignKey("id")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("id")]
        public int RoleId { get; set; }

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }
    }
}