using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table(name: "RoomType")]
    public class RoomType
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActived { get; set; } = true;

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}