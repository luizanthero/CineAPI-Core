using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public bool IsRevoked { get; set; }

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }

        public virtual User User { get; set; }
    }
}