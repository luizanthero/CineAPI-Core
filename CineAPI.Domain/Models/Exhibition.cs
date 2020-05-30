using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineAPI.Models
{
    [Table(name: "Exhibition")]
    public class Exhibition
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("id")]
        public int FilmId { get; set; }

        [Required]
        [ForeignKey("id")]
        public int RoomId { get; set; }

        [Required]
        [ForeignKey("id")]
        public int ScheduleId { get; set; }

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }

        public virtual Film Film { get; set; }
        public virtual Room Room { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}