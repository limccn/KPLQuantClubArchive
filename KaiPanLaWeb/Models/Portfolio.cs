namespace KaiPanLaWeb.Models
{
    public class Portfolio
    {
        public int RN { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string XHGXSJ { get; set; }
        public string XHQRSJ { get; set; }
        public string CODE { get; set; } = "";
        public string NAME { get; set; } = "";
        public double RATE { get; set; }
        public double PRICE { get; set; }
        public double CJE { get; set; }
        public double RATIO { get; set; }
        public double SPEED { get; set; }
        public double SJLTP { get; set; }
        public string TUDE { get; set; } = "";
        public double BUY { get; set; }
        public double SELL { get; set; }
        public double ZLJE { get; set; }
        public double QJZF { get; set; }
        public string TAG { get; set; } = "";

    }
}