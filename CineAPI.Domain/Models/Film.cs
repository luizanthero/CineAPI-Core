using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineAPI.Models
{
    [Table(name: "Film")]
    public class Film
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string ApiCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Year { get; set; }

        [Required]
        [MaxLength(255)]
        public string Released { get; set; }

        [Required]
        [MaxLength(255)]
        public string Runtime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(255)]
        public string Director { get; set; }

        [Required]
        [MaxLength(255)]
        public string Writer { get; set; }

        [Required]
        [MaxLength(255)]
        public string Actors { get; set; }

        [Required]
        [MaxLength(255)]
        public string Plot { get; set; }

        [Required]
        [MaxLength(255)]
        public string Language { get; set; }

        [Required]
        [MaxLength(255)]
        public string Country { get; set; }

        [Required]
        [MaxLength(255)]
        public string Awards { get; set; }

        [Required]
        [MaxLength(255)]
        public string Poster { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; }

        [Required]
        [MaxLength(255)]
        public string Production { get; set; }

        [Required]
        [MaxLength(255)]
        public string Website { get; set; }

        [DefaultValue(true)]
        [JsonIgnore]
        public bool IsActived { get; set; } = true;

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }
    }
}