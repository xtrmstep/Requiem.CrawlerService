using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class DataBlock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public UrlItem Url { get; set; }
        public DataBlockType Type { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
    }
}