using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class ExtractRule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DomainName ForDomain { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string RegExpression { get; set; }
    }
}