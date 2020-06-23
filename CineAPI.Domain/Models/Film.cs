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
        public string Name { get; set; }

        [Required]
        public string ApiCode { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public string Released { get; set; }

        [Required]
        public string Runtime { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string Writer { get; set; }

        [Required]
        public string Actors { get; set; }

        [Required]
        public string Plot { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Awards { get; set; }

        [Required]
        public string Poster { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Production { get; set; }

        [Required]
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