using System;
using System.Collections.Generic;

namespace KaiPanLaWeb.Models
{
    public class SimulateTrades
    {
        public Int64 TTL { get; set; }
        public Int64 Timestamp { get; set; }
        public List<SimulateTrade> Detail { get; set; }
    }
}