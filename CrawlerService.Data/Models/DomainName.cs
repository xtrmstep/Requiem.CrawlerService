using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class DomainName
    {
        [Key]
        public string Name { get; set; }

        public DateTime? EvaliableFromDate { get; set; }

        [Column("Allow")]
        public string SerializedAllow { get; set; }

        [Column("Disallow")]
        public string SerializedDisallow { get; set; }

        public float? CrawlDelay { get; set; }
        public IList<Process> Processes { get; set; } = new List<Process>();
        public IList<ExtractRule> ExtractRules { get; set; } = new List<ExtractRule>();

        #region Not mapped

        public string[] Allow
        {
            get { return SerializedAllow?.Split(';'); }
            set { SerializedAllow = string.Join(";", value); }
        }

        public string[] Disallow
        {
            get { return SerializedDisallow?.Split(';'); }
            set { SerializedDisallow = string.Join(";", value); }
        }

        #endregion
    }
}