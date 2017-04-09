using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class UrlItem
    {
        public UrlItem()
        {
            Jobs = new List<JobItem>();
            DataBlocks = new List<DataBlock>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get;
            set;
        }

        /// <remarks>It should be unique for whole set</remarks>
        // todo test for uniqueness
        public string Url
        {
            get;
            set;
        }

        public string Host // todo rename to Host
        {
            get;
            set;
        }

        public bool IsInProgress
        {
            get;
            set;
        }

        public DateTime? EvaliableFromDate
        {
            get;
            set;
        }

        public ICollection<JobItem> Jobs
        {
            get;
            set;
        }

        public ICollection<DataBlock> DataBlocks
        {
            get;
            set;
        }
    }
}