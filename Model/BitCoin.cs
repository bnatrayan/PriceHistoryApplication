using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceHistoryApplication.Model
{
    public class BitCoin
    {
        public string symbol { get; set; }
        public string price_24h { get; set; }
        public string volume_24h { get; set; }
        public string last_trade_price { get; set; }
    }
}
