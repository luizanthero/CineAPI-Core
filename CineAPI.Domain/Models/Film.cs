using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table("Film")]
    public class Film
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ApiCode { get; set; }

        [DefaultValue(true)]
        public bool IsActived { get; set; } = true;

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
