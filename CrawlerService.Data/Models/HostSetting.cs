using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class HostSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <remarks>It should be unique for whole set</remarks>
        // todo test for uniqueness
        public string Host { get; set; }

        public string RobotsTxt { get; set; }
        public string Disallow { get; set; }
        public int CrawlDelay { get; set; }
    }
}