using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class Process
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DomainName Domain { get; set; }
        public string Status { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
    }
}