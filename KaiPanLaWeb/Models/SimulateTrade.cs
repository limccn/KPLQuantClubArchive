using System;

namespace KaiPanLaWeb.Models
{


    public class SimulateTradeReq
    {
        public string DATE { get; set; }
        public string TIME { get; set; }
        public Int32 NONCE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string SIDE { get; set; }
        public string ACTION { get; set; }
        public string TRADE_TYPE { get; set; }
        public double PRICE { get; set; }
        public double NUMBER { get; set; }
        public double AMOUNT { get; set; }
        public double FEE { get; set; }
        public double BALANCE { get; set; }

    }

    public class SimulateTrade : SimulateTradeReq
    {
        public string APP { get; set; }
        public string CHANNEL { get; set; }
        public string TYPE { get; set; }
        //public string DATA { get; set; }

        public double ORDER_PRICE { get; set; }



    }
}