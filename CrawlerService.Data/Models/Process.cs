using System;
using System.ComponentModel.DataAnnotations.Schema;
using CrawlerService.Common.DateTime;

namespace CrawlerService.Data.Models
{
    public class Process
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DomainName Domain { get; set; }
        public string Status { get; set; }
        public DateTime DateStart { get; set; } = CrawlerDateTime.Now;
        public DateTime? DateFinish { get; set; }
    }
}