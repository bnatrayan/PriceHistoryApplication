using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceHistoryApplication.Model
{
    public class PriceData
    {
        public DateTime time { get; set; }
        public Double price { get; set; }
    }
    public class PriceDataUnix
    {
        public long time { get; set; }
        public Double price { get; set; }
    }
}
