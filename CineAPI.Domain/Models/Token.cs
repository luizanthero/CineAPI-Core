using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table(name: "Token")]
    public class Tokens
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("id")]
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public bool IsRevoked { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public virtual User User { get; set; }
    }
}