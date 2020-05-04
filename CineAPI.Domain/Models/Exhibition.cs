using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table("exhibitions")]
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

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
