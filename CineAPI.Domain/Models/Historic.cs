using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Models
{
    [Table(name: "Historic")]
    public class Historic
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public int TableKey { get; set; }

        public string JsonBefore { get; set; }

        public string JsonAfter { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("id")]
        public int HistoricTypeId { get; set; }

        public virtual HistoricType HistoricType { get; set; }
    }
}