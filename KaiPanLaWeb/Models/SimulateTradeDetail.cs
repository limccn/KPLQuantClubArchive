namespace KaiPanLaWeb.Models
{
    public class SimulateTradeDetail : SimulateTrade
    {

        public int RN { get; set; }
        public string SBSJ { get; set; }
        public string XHQRSJ { get; set; }
        public string TUDE { get; set; }

        public double RATE { get; set; }
        public double CJE { get; set; }
        public double RATIO { get; set; }
        public double SPEED { get; set; }
        public double SJLTP { get; set; }
        public double BUY { get; set; }
        public double SELL { get; set; }
        public double ZLJE { get; set; }
        public double QJZF { get; set; }

        public double TL { get; set; }
        public double QD { get; set; }
    }
}