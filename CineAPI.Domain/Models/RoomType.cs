using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table("room_types")]
    public class RoomType
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActived { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
