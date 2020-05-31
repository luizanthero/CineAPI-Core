using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineAPI.Models
{
    [Table(name: "Role")]
    public class Role
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue(true)]
        [JsonIgnore]
        public bool IsActived { get; set; } = true;

        [JsonIgnore]
        public DateTime created_at { get; set; }

        [JsonIgnore]
        public DateTime updated_at { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}