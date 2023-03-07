# 
# 更新日志
# 2020-11-12 第一版
# 2020-11-13 排除最近3日涨幅超过15%的股票
# 2020-11-17 增加仓位配置参数,增加信号触发条件
# 2020-11-26 调整筛选算法,支持两个市场
# 2020-12-14 调整策略筛选算法, 使用服务端筛选, 进行信号二次确认
# 2020-12-15 增加卖出监控策略, 仓位控制策略
# 2020-12-17 调整信号抓取时间区间，使用延迟10秒结果
# 2020-12-18 盘中监控bug修复
# =============================================
# 2020-12-28 转换为python2.0版本，未改动

# 导入函数库
from jqdata import *
import requests
import numpy as np
import pandas as pd
import json
import datetime
import random

# 初始化函数,设定基准等等
def initialize(context):
    # 设定沪深300作为基准
    set_benchmark('000300.XSHG')
    # 开启动态复权模式(真实价格)
    set_option('use_real_price', True)
    # 输出内容到日志 log.info()
    log.info('初始函数开始运行且全局只运行一次')
    # 过滤掉order系列API产生的比error级别低的log
    # log.set_level('order', 'error')
    ### 股票相关设定 ###
    # 股票类每笔交易时的手续费是：买入时佣金万分之三,卖出时佣金万分之三加千分之一印花税, 每笔交易佣金最低扣5块钱
    set_order_cost(OrderCost(close_tax=0.001, open_commission=0.0003, close_commission=0.0003, min_commission=5), type='stock')

    ## 运行函数（reference_security为运行时间的参考标的；传入的标的只做种类区分,因此传入'000300.XSHG'或'510300.XSHG'是一样的）
      # 开盘前运行
    run_daily(before_market_open, time='before_open', reference_security='000300.XSHG')
    # 开盘清仓
    #run_daily(market_open, time='open', reference_security='000300.XSHG')
    run_daily(market_open_sell_all, time='9:30', reference_security='000300.XSHG')
    
    # 每天10点二十分左右，所有现金买入逆回购
    #run_daily(market_open_sell_cash_drr, time='10:20', reference_security='000300.XSHG')
    
    # 收盘前清仓今日未涨停的股票
    run_daily(market_close_sell_all, time='14:45', reference_security='000300.XSHG')
    
      # 收盘后运行
    run_daily(after_market_close, time='after_close', reference_security='000300.XSHG')

## 开盘前运行函数
def before_market_open(context):
    # 输出运行时间
    log.info('函数运行时间(before_market_open)：'+str(context.current_dt.time()))
    
    # 给微信发送消息（添加模拟交易,并绑定微信生效）
    send_message('美好的一天~')
    
    log.info('开盘时间,清空当日api数据')
    context.today_stocks_datas = None
    
    # 获取上证指数000001.XSHG/创业板指数10日均线
    ma1,ma2 = compute_last_n_days_ma(context,'399006.XSHE',10)
    # 指数均线是否向下
    context._index_ma_is_down = (ma1 <= ma2)
    if context._index_ma_is_down:
        log.info('before_market_open, 创业板指数10日均线【向下】, 单笔持仓不超过【5%】')
        context._order_cash_ratio = 0.05              # 单股单次交易金额占比,不超过可用现金的5%
    else:
        log.info('before_market_open, 创业板指数10日均线【向上】, 单笔持仓不超过【10%】')
        context._order_cash_ratio = 0.10              # 单股单次交易金额占比,不超过可用现金的10%
    
    context._mini_per_order_cash = 10000          # 最小单笔交易金额
    context._mini_cash = 5000                    # 最低持有现金数,默认5000
    context._per_order_cash = 10000              # 单笔交易金额
    
    context._today_stock_buy_count = 0           # 当日买入次数
    context._max_today_stock_buy_count = 5       # 当日买入最大次数, 不超过4
    
    log.info('before_market_open, 单日最大买入次数：'+str(context._max_today_stock_buy_count))
    
    context._debug = False
    if context._debug:
        log.info('============DEBUG 模式============')
    else:
        log.info('============ON GO 模式============')

    # 要操作的股票：平安银行（g.为全局变量）
    #g.security = '000001.XSHE'
    
def handle_data(context,data):
    log.info('函数运行时间(handle_data):'+str(context.current_dt.time()))
    
    #===================以下为卖出交易策略=================================
    if not is_before_close_time(context):
        log.info('handle_data, 非卖出策略盯盘时间,9:30~14:45')
        return
    #循环遍历现有持仓的个股
    for k,v in context.portfolio.positions.items():
        # 卖出今日盘中跌破五个点的个股
        if v.closeable_amount > 0:
            #获取当前tick价格
            tick = get_ticks(k, end_dt=context.current_dt, count=1, \
                            fields=['time', 'current'], skip=True, df=True)
            # 获取前日收盘价
            #last_price = get_price(k, end_date=context.previous_date, \
            #                    frequency='daily', fields=['close'], \
            #                    skip_paused=True, fq='pre', count=1)
                                
            #获取前日收盘价
            last_price = get_price(k, end_date = context.current_dt, \
                        fields = ['high_limit','pre_close'], skip_paused=True, \
                        frequency='daily', fq='pre', count = 1)
                        
            # 今日盘中未跌破五个点, 继续持仓          
            if tick['current'][0] - last_price['pre_close'][0] * 0.95 >= 0:
                log.info('handle_data, 盘中【未跌破】5个点, 继续持仓 '+\
                            ', 股票代码：'+str(k)+\
                            ', 当前价:'+str(tick['current'][0]) + \
                            ', 昨日收盘价:'+str(last_price['pre_close'][0]) + \
                            ', 昨收价*[0.95]:'+str(last_price['pre_close'][0] * 0.95))
                continue
            #今日盘中跌破五个点, 斩仓出局  
            log.info("handle_data, 卖出盘中【跌破】五个点, 股票代码:" + str(k))
            # 卖出所有股票,使这只股票的最终持有量为0
            order_target(k, 0)
    
    #===================以下为买入交易策略=================================
    #  如果不是交易时间,直接返回
    if not is_business_time(context):
        log.info('handle_data, 非买入策略盯盘时间,9:30~9:50')
        return
    stocks_codes = get_api_stocks(context)
    if len(stocks_codes) == 0:
        log.info('handle_data, 未出现交易信号,等待下一个交易信号。')
        return
    cash = context.portfolio.available_cash
    log.info('handle_data, 获得交易信号:'+ str(stocks_codes) + \
                        ', 当前可用现金:'+ str(cash))
    # 单笔交易限额必须大于0
    order_cash = context._per_order_cash
    
    if stocks_codes and order_cash > 0:
        # 过滤上市不满30天的新股
        stocks_codes = filter_stock_list_days(context, stocks_codes, 30)
        if len(stocks_codes) == 0:
            return
        # 过滤ST股
        stocks_codes = filter_stock_is_st(context, stocks_codes)
        if len(stocks_codes) == 0:
            return
        # 循环遍历符合条件的股票
        for stock in stocks_codes:
            # 已经在portfolio的股票,跳过
            if stock in context.portfolio.positions:
                continue
            # 获得昨天和前天的价格
            day2price = get_price(stock, end_date=context.previous_date, \
                                    frequency='daily', fields=['close'], \
                                    skip_paused=True, fq='pre', count=2)
            # 获取前日收盘价
            last_close = day2price['close'][1]
            last_change = (day2price['close'][1] - day2price['close'][0]) / day2price['close'][0]
            
            #昨日涨幅>6% 
            if last_change >= 0.06:
                log.info('handle_data, 昨日涨幅>【6%】, 放弃买入'+\
                    ', 股票代码：'+str(stock) +\
                    ', 昨日价格:'+ str(last_close) +\
                    ', 昨日涨跌幅：'+str(last_change))
                continue
            
            #获取当前tick价格
            tick = get_ticks(stock, end_dt=context.current_dt, count=1, \
                            fields=['time', 'current', 'high', 'low'], skip=True, df=True)
            
            #今日最低价低于昨日收盘5%,底部反弹
            if (tick['low'][0] - last_close) / last_close <= -0.05:
                log.info('handle_data, 今日最低价低于昨日收盘【5%】, 底部反弹, 放弃买入'+\
                    ', 股票代码：'+str(stock)+\
                    ', 今日最低:'+ str(tick['low'][0]) +\
                    ', 昨日收盘：'+str(last_close))
                continue
            
            #今日最高价高于昨日收盘8%,高位回落
            if (tick['high'][0] - last_close) / last_close >= 0.08:
                log.info('handle_data, 今日最高价高于昨日收盘【8%】, 高位回落, 放弃买入'+\
                    ', 股票代码：'+str(stock)+\
                    ', 今日最高:'+ str(tick['high'][0]) +\
                    ', 昨日收盘：'+str(last_close))
                continue
            
            # 最近5日涨幅不能超过20%
            rate_5d = compute_last_n_days_rate(context, stock, 5)
            log.info('handle_data, 股票代码:'+stock +\
                    '最近5日涨幅:'+ str(rate_5d))
            if rate_5d > 1.20:
                log.info('handle_data, 最近5日涨幅超过【20%】, 放弃买入'+\
                                   ', 股票代码：'+str(stock)+\
                                   ', 股票代码:'+ stock +\
                                   ', 5日涨幅:' + str(rate_5d))
                continue
            
            # 10日均线不能向下
            ma1,ma2 = compute_last_n_days_ma(context,stock,10)
            #log.info('handle_data, 10日均线,昨日:'+str(ma1) + ',前日：'+str(ma2))
            if ma1 <= ma2:
                log.info('handle_data, 10日均线【向下】, 放弃买入'+\
                        ', 股票代码：'+str(stock)+\
                        ', 昨日MA20:'+str(ma1) + \
                        ', 前日MA20:'+str(ma2))
                continue
            
            # 达到当日买入限制次数
            if context._today_stock_buy_count >= context._max_today_stock_buy_count:
                log.info('handle_data, 已达到当日交易限制次数：'+str(context._today_stock_buy_count))
                continue
            
            # 买入计数+1
            context._today_stock_buy_count = context._today_stock_buy_count + 1
            log.info('handle_data, 最大买入次数：'+str(context._max_today_stock_buy_count)+\
                                  ',当前买入次数：'+str(context._today_stock_buy_count))
                                  
            log.info('handle_data, 下单【买入】, 股票代码:'+stock +\
                                    ', 买入金额：'+str(order_cash))
            order_value(stock, order_cash)

## 开盘时运行函数
def market_open_sell_all(context):
    log.info('函数运行时间(market_open_sell_all):'+str(context.current_dt.time()))
    for k,v in context.portfolio.positions.items():
        # 如果指数向上, 判断是否继续持有
        if not context._index_ma_is_down:
            log.info('market_open_sell_all, 创业板指数10日均线【向上】, 检查持仓个股昨日是否涨停')
            previous_date = context.previous_date
            #获得前一交易日的数据
            prices = get_price(k, end_date = previous_date, \
                     fields = ['close','high_limit','pre_close'], \
                     skip_paused=True, count = 1)
            
            # 如果昨日涨停, 收盘价等于涨停价
            if prices['close'][0] - prices['high_limit'][0] >= 0:
                log.info('market_open_sell_all, 昨日【涨停】, 早盘继续持有'+\
                            ', 股票代码：'+str(k)+\
                            ', 昨日收盘价:'+str(prices['close'][0]) + \
                            ', 昨日涨停价:'+str(prices['high_limit'][0]))
                continue
        else:
            log.info('market_open_sell_all, 创业板指数10日均线【向下】, 清仓【卖出】所有个股')
        # 昨日未涨停的个股今天直接卖出
        if v.closeable_amount > 0:
            log.info('market_open_sell_all, 开盘清仓【卖出】昨日未涨停的股票'+
                        ', 股票代码：'+str(k)+\
                        ', 昨日收盘价:'+str(prices['close'][0]) + \
                        ', 昨日涨停价:'+str(prices['high_limit'][0]))
            # 卖出所有股票,使这只股票的最终持有量为0
            order_target(k, 0)
            
    log.info('market_open_sell_all, 当日可用现金:' + str(context.portfolio.available_cash))
    context._per_order_cash = compute_order_cash(context)
    log.info('market_open_sell_all, 当日单笔交易限额：' + str(context._per_order_cash))
    
## 开盘时运行函数
def market_close_sell_all(context):
    log.info('函数运行时间(market_close_sell_all):'+str(context.current_dt.time()))
    for k,v in context.portfolio.positions.items():
        #当日未涨停的股票, 下午提前卖出
        if v.closeable_amount > 0:
            #获取当前tick价格
            tick = get_ticks(k, end_dt=context.current_dt, count=1, \
                    fields=['time', 'current'], skip=True, df=True)
            #获取个股当日涨停价格
            prices = get_price(k, end_date = context.current_dt, \
                              fields = ['high_limit','pre_close'], \
                              skip_paused=True, fq='pre', count = 1)
                            
            # 如果在14:54分股票涨停, 则当日继续持有
            if tick['current'][0] - prices['high_limit'][0] >= 0:
                log.info('market_open_sell_all, 14:45收盘前涨停打板的个股, 继续持有'+\
                            ', 股票代码：'+str(k)+\
                            ', 当前价:'+str(tick['current'][0]) +\
                            ', 涨停价:'+str(prices['high_limit'][0]))
                continue
            # 如果在14:54分股票没有涨停, 则当日清仓
            log.info('market_open_sell_all, 14:54收盘前未涨停的个股, 清仓【卖出】'+\
                            ', 股票代码：'+str(k)+\
                            ', 当前价:'+str(tick['current'][0]) +\
                            ', 涨停价:'+str(prices['high_limit'][0]))
            # 卖出所有股票,使这只股票的最终持有量为0
            order_target(k, 0)
            
## 收盘后运行函数
def after_market_close(context):
    log.info(str('函数运行时间(after_market_close):'+str(context.current_dt.time())))
    log.info('#########  一天结束  ###########')
    log.info('当日可用现金：' + str(context.portfolio.available_cash))
    log.info('当日资产总额：' + str(context.portfolio.total_value))
    #得到当天所有成交记录
    trades = get_trades()
    for _trade in trades.values():
        log.info('成交记录：'+str(_trade))
    
    log.info('收盘时间,清空当日api数据')
    context.today_stocks_datas = None
    log.info('#################  我是分割线   ###################')
    
def get_api_stocks(context):
    if not context._debug:
        diff = datetime.datetime.now().date() - context.current_dt.date()
    else:
        log.info('DEBUG模式,测试当天数据')
        diff = datetime.datetime.now().date() - datetime.datetime.now().date()
        
    stocks = []
    # 如果相差大于等于1天, 非当日回测交易
    if diff.days >= 1:
        log.info('get_api_stocks, 非当日回测交易,API只取一次')
        api_data = context.today_stocks_datas
        if api_data is None:
            api_data = request_quanshow_data_api(context)
            log.info('API服务器返回当日数据：%s'%str(api_data))
            context.today_stocks_datas = api_data
        stocks = filter_request(context,api_data)    
    else:
        if is_business_time(context):
            api_data = request_quanshow_data_api(context)
            stocks = filter_request(context,api_data)
        else:
            log.info('get_api_stocks, 当日交易,非交易时间段')
            stocks = []
    return stocks

def request_quanshow_data_api(context):
    if not context._debug:
        dat = context.current_dt.strftime('%Y%m%d')
    else:
        log.info('DEBUG模式,测试当天数据')
        dat = datetime.datetime.now().strftime('%Y%m%d')

    url = 'https://api.limc.cn/api/Signal'
    params ={"ttl":0, "count":20,"date":dat,'_rnd':random.randint(10000000,99999999)}
    headers = {
        'Content-Type':'application/json; charset=UTF-8',
        'Host':'api.limc.cn',
        'Origin':'https://www.joinquant.com/',
        'Referer':'https://www.joinquant.com/',
        'User-Agent':'Mozilla/5.0 (Linux; Android 6.0;) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Mobile Safari/537.36 QuantShow/0.1.201113',
        'Accept': 'application/json, text/javascript, */*; q=0.01',
    }
    log.info('[API]向API服务器发起请求, @'+str(datetime.datetime.now()))
    r = requests.get(url,params=params,headers=headers)
    log.info('[API]API服务器返回数据, @'+str(datetime.datetime.now()))
    return r.text
    
def compute_last_n_days_rate(context, stock, days):
    # 获取五日涨跌幅
    df = attribute_history(stock, days, '1d', ['close','pre_close'])
    quote_rates = ((df.close - df.pre_close) / df.pre_close) + 1.0
    rate_days = 1.0
    # 循环
    for rate in quote_rates:
        rate_days = rate_days * rate
    return rate_days
    
def compute_last_n_days_ma(context, stock, days):
    #获取前一日日期
    date = context.previous_date
    #获得days+1的数据
    prices = get_price(stock, end_date = date, fields = 'close',\
                        skip_paused=True, count = days + 1)
    close_ma = prices['close']
    #获取前一交易日日均线数据
    MA1 = close_ma[1:].mean()
    MA2 = close_ma[:-1].mean()
    return MA1,MA2
    
def filter_stock_list_days(context, stocks, delta):
    '''过滤新股'''   
    date = context.current_dt.date()
    result = [stock for stock in stocks if (date - get_security_info(stock).start_date) > datetime.timedelta(delta)]
    if len(stocks) > len(result):
        log.info('filter_stock_list_days, 剔除新股数据'+\
                ', 输入：'+str(stocks)+\
                ', 输出：'+str(result))
    return result
    
def filter_stock_is_st(context, stocks):
    '''过滤ST'''
    date = context.current_dt.date()
    datas = get_extras('is_st',stocks,end_date = date ,count=1).T
    result = datas[~datas.iloc[:,0]].index.tolist()
    if len(stocks) > len(result):
        log.info('filter_stock_is_st, 剔除ST股数据'+\
                ', 输入：'+str(stocks)+\
                ', 输出：'+str(result))
    return result

def get_stocks_filtered(stocklist,tradedate):
    # 判断当天是否是st,返回的是df,内容为False
    is_st = get_extras('is_st',stocklist,end_date=tradedate,count=1).T
    # 判断当天是否全天停牌,返回的是df
    # 判断上市日期大于30天,大于返回False
    is_susp = get_price(stocklist,end_date=tradedate, count=1,\
                fields='paused',panel=False).set_index('code')[['paused']]
    is_susp = is_susp == 1
    # 拼接前两个df,再新建一列is_short
    con_df = pd.concat([is_st, is_susp], axis=1)
    # 判断每行只要有True的就返回True,切取3个都是False的行(股票)
    stock_filtered = con_df[~con_df.any(axis=1)].index.tolist()
    return stock_filtered
    
def compute_order_cash(context):
    # 可用现金
    cash = context.portfolio.available_cash
    # 判断可用现金是否满足z
    if cash <= context._mini_cash:
        log.info('compute_order_cash, 现金不满足最低仓位现金限制,当日限制交易')
        return 0
    if cash <= context._mini_cash + context._mini_per_order_cash:
        log.info('compute_order_cash, 现金不满足最低单笔交易限制,当日限制交易')
        return 0
    
    # 仓位比例计算单笔交易金额
    max_cash_to_order = context.portfolio.total_value * context._order_cash_ratio
    # 单笔交易金额,如果小于最低金额,则返回最低金额
    per_order_cash = max_cash_to_order  \
                if max_cash_to_order > context._mini_per_order_cash \
                else context._mini_per_order_cash
    
    log.info('compute_order_cash, 最低仓位现金限制：' + str(context._mini_cash) +\
                ', 单笔交易限制比率：' + str(context._order_cash_ratio) +\
                ', 单笔交易限制金额：' + str(context._per_order_cash))
             
    return per_order_cash
    

def is_business_time(context):
    ''' 9:30～9：50 判断为买入盯盘时间段, 周期触发到51分''' 
    tim = context.current_dt.time()
    return (tim.hour == 9) and (tim.minute >= 30) and (tim.minute <= 51)
    
def is_before_close_time(context):
    ''' 14:45 之前判断为卖出盯盘时间段''' 
    tim = context.current_dt.time()
    if(tim.hour < 14):
        return True
    else:
        return tim.minute <= 45
    
def reformat_stock_code(code):
    str_code = str(code).zfill(6)
    if str_code[0:2] in ['60','68','90'] :
        return str_code + '.XSHG'
    if str_code[0:2] in ['00','30','20'] :
        return str_code + ".XSHE"
    

def filter_alorithm_1(context, df):
    tim = context.current_dt.time()
    #上榜时间
    sbsj_d = tim.hour * 10000 + (tim.minute - 1) * 100 + tim.second
    sbsj_u = tim.hour * 10000 + tim.minute * 100 +  tim.second  
    log.info('filter_alorithm_1, 信号确认时间段：【'+str(sbsj_d) + '】 ～ 【' + str(sbsj_u)+'】')
    
    #涨幅 2到7
    #深圳,创业板, 不包含创业板
    #只取1分钟
    fi = df[(df.RATE >= 2.0) & (df.RATE <= 7.0) & \
        (df.CODE > 0) & (df.CODE < 688000) & \
        (df.SBSJ >= sbsj_d) & (df.SBSJ <= sbsj_u)] 
    
    #信号1：强度 > 100 AND 主力买入 1500 买入
    #信号2：强度 > 60 AND  主力净额 > 2000万   买入
    #信号3：体量 > 1200 AND 主力净额 > 2000万 买入
    cond1 = fi[(fi.QD >= 100) & (fi.BUY >= 1500 * 10000)]
    if len(cond1.CODE) > 0:
        log.info('>>>信号1：强度 > 100 AND 主力买入 1500万 买入:' + \
        str([reformat_stock_code(a) for a in cond1.CODE]))
    
    cond2 = fi[(fi.QD >= 60) & (fi.ZLJE >= 2000 * 10000)]
    if len(cond2.CODE) > 0:
        log.info('>>>信号2：强度 >  60 AND 主力净额 > 2000万 买入:' + \
            str([reformat_stock_code(a) for a in cond2.CODE]))
            
    cond3 = fi[(fi.TL >= 1200) & (fi.ZLJE >= 2000 * 10000)]
    if len(cond3.CODE) > 0:
        log.info('>>>信号3：体量 > 1200 AND 主力净额 > 2000万 买入:' + \
            str([reformat_stock_code(a) for a in cond3.CODE]))
    
    # 合并所有信号
    all_fi = pd.concat([cond1,cond2,cond3])
    # 去除重复的
    all_fi.drop_duplicates(['CODE'],keep="last")
    
    #all_fi = pd.DataFrame(cond1)
    #all_fi.append(cond1)
    #all_fi.append(cond2)
    #all_fi.append(cond3)
    
    return all_fi

def filter_alorithm_2(context, df):
    tim = context.current_dt.time()
    #信号确认时间, 接收延迟10s以内的数据, 093010～093109
    xhqrsj_d = tim.hour * 10000 + (tim.minute - 1) * 100 + 10
    xhqrsj_u = tim.hour * 10000 + tim.minute * 100 + 9
    log.info('filter_alorithm_2, 信号确认时间段：【'+str(xhqrsj_d) + '】 ～ 【' + str(xhqrsj_u)+'】')
    #涨幅  不限制
    #深圳,创业板, 不包含科创板
    #只取1分钟
    fi = df[(df.CODE > 0) & (df.CODE < 688000) & \
            (df.XHQRSJ >= xhqrsj_d) & (df.XHQRSJ <= xhqrsj_u)] 
            
    return fi
    
def filter_request(context,data):
    df = pd.read_json(data)
    if not df.empty:
        # 使用策略2 筛选
        filtered_fi = filter_alorithm_2(context, df)
        #使用append方法无需去重
        #all_fi.drop_duplicates(['CODE'],keep="last")
            
        stocks_codes = [reformat_stock_code(a) for a in filtered_fi.CODE]
        return stocks_codes;
    return []
    