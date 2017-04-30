using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerService.Common.DateTime
{
    public static class CrawlerDateTime
    {
        public static System.DateTime Now => System.DateTime.UtcNow;
    }
}
