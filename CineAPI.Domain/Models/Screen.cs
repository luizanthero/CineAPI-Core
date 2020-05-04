using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CineAPI.Domain.Models
{
    [Table("screens")]
    public class Screen
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Size { get; set; }

        [DefaultValue(true)]
        public bool IsActived { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
