namespace KaiPanLaWeb.Models
{
    public class Plate
    {
        public int RN { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public double QD { get; set; }//强度  -792.354,
        public double RATE { get; set; }//涨跌幅  -1.924,
        public double SPEED { get; set; }//涨速  0.266,
        public double CJE { get; set; }//成交额  13024993013,
        public double ZLJE { get; set; }//主力净额  -711567348,
        public double BUY { get; set; }//主力买入  2529322470,
        public double SELL { get; set; } //主力卖出  -3240889818,
        public double LB { get; set; }//量比  0.843,
        public double LTSZ { get; set; }//流通市值  314310954615,
        public double QJZF { get; set; }//涨跌幅  -1.924,
        public string LASTPRICE { get; set; } //XXXX  "891.412"
    }
}