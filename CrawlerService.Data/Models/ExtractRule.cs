using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class ExtractRule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DomainName Domain { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string RegExpression { get; set; }
    }
}