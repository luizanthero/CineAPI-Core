using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineAPI.Models
{
    [Table(name: "Room")]
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
        [JsonIgnore]
        public bool IsActived { get; set; } = true;

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }

        public virtual RoomType RoomType { get; set; }
        public virtual Screen Screen { get; set; }
    }
}