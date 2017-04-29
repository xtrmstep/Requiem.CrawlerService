using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class ActivityMessage
    {
        public ActivityMessage()
        {
            Date = DateTime.UtcNow;
            Type = ActivityMessageType.Message;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Message { get; set; }
        public DateTime Date { get; set; }
        public ActivityMessageType Type { get; set; }
    }
}