using System;
using System.ComponentModel.DataAnnotations.Schema;
using CrawlerService.Common.DateTime;

namespace CrawlerService.Data.Models
{
    public class DataBlock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime ExtractedDate { get; set; } = CrawlerDateTime.Now;
    }
}