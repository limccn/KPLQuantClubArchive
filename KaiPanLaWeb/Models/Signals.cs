using System;
using System.Collections.Generic;

namespace KaiPanLaWeb.Models
{
    public class Signals
    {
        public Int64 TTL { get; set; }
        public Int64 Timestamp { get; set; }
        public List<SignalDetail> Detail { get; set; }
    }
}