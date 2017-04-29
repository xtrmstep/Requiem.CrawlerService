using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class CrawlRule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }

        public string Name { get; set; }
        public DataBlockType DataType { get; set; }
        public string RegExpression { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CrawlRule;
            if (obj == null || other == null)
            {
                return false;
            }

            return Id == other.Id
                   && Name == other.Name
                   && DataType == other.DataType
                   && RegExpression == other.RegExpression;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}