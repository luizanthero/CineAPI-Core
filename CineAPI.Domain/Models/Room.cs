using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("id")]
        public int RoomTypeId { get; set; }

        [Required]
        [ForeignKey("id")]
        public int ScreenId { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool IsActived { get; set; } = true;

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public virtual RoomType RoomType { get; set; }
        public virtual Screen Screen { get; set; }
    }
}
