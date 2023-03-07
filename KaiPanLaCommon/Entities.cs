using System;
using System.Collections.Generic;

namespace KaiPanLaCommon
{

    public class FilterCondition
    {
        public bool enabled { get; set; } = false;
    }

    public class SignalFilterCondition : FilterCondition
    {
        public Int64 QD { get; set; } = 0;
        public Int64 TL { get; set; } = 0;
        public Int64 BUY { get; set; } = 0;
        public Int64 ZLJE { get; set; } = 0;
    }

    public class PortSignalFilterCondition : FilterCondition
    {
        public Int64 SJLTP_FROM { get; set; } = 0;
        public Int64 SJLTP_TO { get; set; } = 0;
        public Int64 RATE_FROM { get; set; } = 0;
        public Int64 RATE_TO { get; set; } = 0;
        public bool SJLTP_CEFF { get; set; } = false;
        public Int64 BUY { get; set; } = 0;
        public Int64 ZLJE { get; set; } = 0;
    }

    public class StockRankingRequest
    {
        public string c { get; set; } = "StockRanking";
        public string a { get; set; } = "RealRankingInfo";
        public string Date { get; set; } = "";
        public string RStart { get; set; } = "";
        public string REnd { get; set; } = "";
        public int Ratio { get; set; } = 2;
        public int Type { get; set; } = 1;
        public int Order { get; set; } = 1;
        public int index { get; set; } = 0;
        public int st { get; set; } = 50;
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }


    public class PortfolioRequest
    {
        public string c { get; set; } = "StockRanking";
        public string a { get; set; } = "SelStockRank";
        public string Date { get; set; } = "";
        public string RStart { get; set; } = "";
        public string REnd { get; set; } = "";
        public string Tag { get; set; } = "";
        public int Order { get; set; } = 1;
        public int index { get; set; } = 0;
        public int st { get; set; } = 50;
        public string CombineID { get; set; } = "";
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }


    public class PlateListRequestV1Old
    {
        public string c { get; set; } = "PCArrangeData";
        public string a { get; set; } = "GetZSIndexPlate";
        public string SelType { get; set; } = "2";
        public string ZSType { get; set; } = "7";
        public int PType { get; set; } = 1;
        public int POrder { get; set; } = 1;
        public string PStart { get; set; } = "";
        public string PEnd { get; set; } = "";
        public int PIndex { get; set; } = 0;
        public int Pst { get; set; } = 10;
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }

    public class PlateListRequest
    {
        public string c { get; set; } = "ZhiShuRanking";
        public string a { get; set; } = "RealRankingInfo";
        public string SelType { get; set; } = "2";
        public string ZSType { get; set; } = "7";
        public int Type { get; set; } = 1;
        public int Order { get; set; } = 1;
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public int Index { get; set; } = 0;
        public int st { get; set; } = 10;
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }

    public class PlateStockListRequestV1Old
    {
        public string c { get; set; } = "PCArrangeData";
        public string a { get; set; } = "GetZSIndexPlate";
        public string SelType { get; set; } = "3";
        public int LType { get; set; } = 6;
        public int LOrder { get; set; } = 1;
        public string LStart { get; set; } = "";
        public string LEnd { get; set; } = "";
        public int LIndex { get; set; } = 0;
        public int Lst { get; set; } = 10;
        public string PlateID { get; set; } = "";
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }

    public class PlateStockListRequest
    {
        public string c { get; set; } = "ZhiShuRanking";
        public string a { get; set; } = "ZhiShuStockList_W8";
        public string SelType { get; set; } = "3";
        public int Type { get; set; } = 6;
        public int Order { get; set; } = 1;
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public int Index { get; set; } = 0;
        public int st { get; set; } = 10;
        public string PlateID { get; set; } = "";
        public string UserID { get; set; } = "";
        public string Token { get; set; } = "";
    }


    public class CommonResponseEntty
    {
        public double ttag { get; set; }
        public Int64 errcode { get; set; }
    }

    public class CommonListResponseEntity : CommonResponseEntty
    {
        public Int64 Time { get; set; }
        public Int64 Count { get; set; }
        public List<string> Day { get; set; }
    }

    public class CommonListWithTimePeroidResponseEntity
    {
        public Int64 Time { get; set; }
        public Int64 Count { get; set; }
        public List<string> Day { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
    }

    public class StockRankingsEntity : CommonListResponseEntity
    {
        public List<StockRankingListItemEntity> list { get; set; }
    }

    public class PortfolioEntity : CommonListResponseEntity
    {
        public List<PortfolioListItemEntity> list { get; set; }
    }

    public class PlateListWrapperEntity : CommonResponseEntty
    {
        public PlateListEntity plates { get; set; }
    }

    public class PlateListEntityV1Old : CommonListWithTimePeroidResponseEntity
    {
        public List<Object[]> list { get; set; }
        public List<PlateListItemEntity> parsedList { get; set; }

    }

    public class PlateListEntity : CommonListWithTimePeroidResponseEntity
    {
        public List<Object[]> list { get; set; }
        public List<PlateListItemEntity> parsedList { get; set; }
        public List<String> Title { get; set; }
    }

    public class PlateListItemRemoteEntity
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Code { get; set; }//代码  "801075",
        public string Name { get; set; }//名称  "自主可控",
        public Double QD { get; set; }//强度  -792.354,
        public Double Rate { get; set; }//涨跌幅  -1.924,
        public Double Speed { get; set; }//涨速  0.266,
        public Double CJE { get; set; }//成交额  13024993013,
        public Double ZLJE { get; set; }//主力净额  -711567348,
        public Double Buy { get; set; }//主力买入  2529322470,
        public Double Sell { get; set; } //主力卖出  -3240889818,
        public Double LB { get; set; }//量比  0.843,
        public Double LTSZ { get; set; }//流通市值  314310954615,
        public Double QJZF { get; set; }//涨跌幅  -1.924,
        public string LastPrice { get; set; } //XXXX  "891.412"

    }

    public class PlateListItemEntity : PlateListItemRemoteEntity
    {
        public string _Date { get; set; }
        public string _Time { get; set; }
        public Int64 _Number { get; set; }
        public Double _Rate { get; set; }//涨跌幅  -1.924,
        public Double _Speed { get; set; }//涨速  0.266,
        public Double _CJE { get; set; }//成交额  13024993013,
        public Double _ZLJE { get; set; }//主力净额  -711567348,
        public Double _Buy { get; set; }//主力买入  2529322470,
        public Double _Sell { get; set; } //主力卖出  -3240889818,
        public String _ZLJE_Format { get; set; }//主力净额  -711567348,
        public String _Buy_Format { get; set; }//主力买入  2529322470,
        public String _Sell_Format { get; set; } //主力卖出  -3240889818,
        public Double _LTSZ { get; set; }//流通市值  314310954615,

    }

    public class PlateStockListWrapperEntity : CommonResponseEntty
    {
        public PlateStockListEntity stocks { get; set; }
    }

    public class PlateStockListEntityV1Old : CommonListWithTimePeroidResponseEntity
    {
        public List<Object[]> list { get; set; }
        public List<PlateStockListItemEntity> parsedList { get; set; }
    }

    public class PlateStockListEntity : CommonListWithTimePeroidResponseEntity
    {
        public List<Object[]> list { get; set; }
        public List<PlateStockListItemEntity> parsedList { get; set; }

        public List<string> Stocks { get; set; }
        public List<string> ZB { get; set; }
        public List<string> kzzlist { get; set; }

        public Int32 State { get; set; }
    }


    public class PlateParam
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double QD { get; set; }
    }
    public class PlateStockListItemRemoteEntity
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Code { get; set; }//代码  "301195",
        public string Name { get; set; }//名称  "北路智控",
        public Double Price { get; set; }//价格  83.2,
        public Double Rate { get; set; }//涨幅  11.53,
        public Double CJE { get; set; }//成交  251970747,
        public Double Ratio { get; set; }//换手  14.98,
        public Double Speed { get; set; }//涨速  0.35,
        public Double SJLTP { get; set; }//实际流通  1681276979,
        public Double Buy { get; set; }//主力买  35760446,
        public Double Sell { get; set; }//主力卖  -59217423,
        public Double ZLJE { get; set; }//主力净额  -23456977,
        public Double QJZF { get; set; }//区间涨幅  6.67,
        public string Tude { get; set; }//概念  "新股与次新股、注册制次新股、江苏、融资融券、物联网、计算机应用"
        public string LBCS { get; set; }//连板次数
        public string LT { get; set; }//龙头
        public string EJBK { get; set; }//二级板块
        public Double SPFD { get; set; }//收盘封单
        public Double ZDFD { get; set; }//最大封单
        public Double LZCS { get; set; }//领涨次数
        public Double DDJE300W { get; set; }//大单净额


    }

    public class PlateStockListItemEntity : PlateStockListItemRemoteEntity
    {
        public string PlateID { get; set; }
        public string PlateName { get; set; }
        public double PlateQD { get; set; }

        public Int64 _Number { get; set; }
        public string _Date { get; set; }
        public string _Time { get; set; }
        public Double _Rate { get; set; }//涨幅  11.53,
        public Double _CJE { get; set; }//成交  251970747,
        public Double _Ratio { get; set; }//换手  14.98,
        public Double _Speed { get; set; }//涨速  0.35,
        public Double _SJLTP { get; set; }//实际流通  1681276979,
        public Double _Buy { get; set; }//主力买  35760446,
        public String _Buy_Format { get; set; }//主力买单位格式化,
        public Double _Sell { get; set; }//主力卖  -59217423,
        public String _Sell_Format { get; set; }//主力卖单位格式化,
        public Double _ZLJE { get; set; }//主力净额  -23456977,
        public String _ZLJE_Format { get; set; }//主力净额单位格式化,

        public Double _QJZF { get; set; }//区间涨幅  6.67,

        public Double _SPFD { get; set; }//收盘封单
        public string _SPFD_Format { get; set; }//收盘封单
        public Double _ZDFD { get; set; }//最大封单
        public string _ZDFD_Format { get; set; }//最大封单
        public Double _DDJE300W { get; set; }//大单净额
        public string _DDJE300W_Format { get; set; }//大单净额


    }

    public class CommonRemoteListItemEntity
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Price { get; set; }
        public double CJE { get; set; }
        public double Ratio { get; set; }
        public double Speed { get; set; }
        public double SJLTP { get; set; }
        public string Tude { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double ZLJE { get; set; }
        public double QJZF { get; set; }
    }

    public class CommonListItemEntity : CommonRemoteListItemEntity
    {
        public string _Date { get; set; }
        public string _Time { get; set; }
        public double _Rate { get; set; }
        public double _CJE { get; set; }
        public double _Ratio { get; set; }
        public double _SJLTP { get; set; }
        public double _Buy { get; set; }
        public double _Sell { get; set; }
        public double _ZLJE { get; set; }
        public double _QJZF { get; set; }
    }

    public class PortfolioListItemEntity : CommonListItemEntity
    {
        public int _Number { get; set; }
    }

    public class PortfolioSignalListItemEntity : PortfolioListItemEntity
    {
        public string XHGXSJ { get; set; }
        public string XHQRSJ { get; set; }
    }

    public class StockRankingListItemEntity : CommonListItemEntity
    {
        public int _Number { get; set; }
        public string Tag { get; set; }
    }

    public class StockAnalyseListItemEntity : StockRankingListItemEntity
    {
        public double JCF { get; set; }
        public double JEF { get; set; }
        public double JEZH { get; set; }
        public double JEZH2 { get; set; }
        public double TL { get; set; }
        public double QD { get; set; }

    }

    public class StockSignalAnalyseListItemEntity : StockAnalyseListItemEntity
    {
        public string SBSJ { get; set; }
        public string XHGXSJ { get; set; }
        public string XHQRSJ { get; set; }

    }

    public class StockSignalListItemEntity : StockSignalAnalyseListItemEntity
    {
        public double ZX_Rate { get; set; }
        public double ZX_Price { get; set; }
        public double ZX_CJE { get; set; }
        public double ZX_Ratio { get; set; }
        public double ZX_Speed { get; set; }
        public double ZX_SJLTP { get; set; }
        public double ZX_Buy { get; set; }
        public double ZX_Sell { get; set; }
        public double ZX_ZLJE { get; set; }
        public double ZX_QJZF { get; set; }
        public double ZX_JCF { get; set; }
        public double ZX_JEF { get; set; }
        public double ZX_JEZH { get; set; }
        public double ZX_JEZH2 { get; set; }
        public double ZX_TL { get; set; }
        public double ZX_QD { get; set; }
    }


    public class PlateStockAnalyseListItemEntity : PlateStockListItemEntity
    {
        public double JCF { get; set; }
        public double JEF { get; set; }
        public double JEZH { get; set; }
        public double JEZH2 { get; set; }
        public double TL { get; set; }
        public double QD { get; set; }

    }

    public class PlateStockSignalListItemEntity : PlateStockAnalyseListItemEntity
    {
        public string SBSJ { get; set; }
        public string XHGXSJ { get; set; }
        public string XHQRSJ { get; set; }

    }

    public class PlateZXStockSignalListItemEntity : PlateStockSignalListItemEntity
    {
        public double ZX_Rate { get; set; }
        public double ZX_Price { get; set; }
        public double ZX_CJE { get; set; }
        public double ZX_Ratio { get; set; }
        public double ZX_Speed { get; set; }
        public double ZX_SJLTP { get; set; }
        public double ZX_Buy { get; set; }
        public double ZX_Sell { get; set; }
        public double ZX_ZLJE { get; set; }
        public String _ZX_Buy_Format { get; set; }
        public String _ZX_Sell_Format { get; set; }
        public String _ZX_ZLJE_Format { get; set; }

        public double ZX_QJZF { get; set; }
        public double ZX_JCF { get; set; }
        public double ZX_JEF { get; set; }
        public double ZX_JEZH { get; set; }
        public double ZX_JEZH2 { get; set; }
        public double ZX_TL { get; set; }
        public double ZX_QD { get; set; }
    }
}
