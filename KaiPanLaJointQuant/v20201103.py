# 
# ????
# 2020-11-12 ???
# 2020-11-13 ????3?????15%???
#
#

# ?????
from jqdata import *
import requests
import numpy as np
import pandas as pd
import json
import datetime
import random

# ?????,??????
def initialize(context):
    # ????300????
    set_benchmark('000300.XSHG')
    # ????????(????)
    set_option('use_real_price', True)
    # ??????? log.info()
    log.info('????????????????')
    # ???order??API????error????log
    # log.set_level('order', 'error')
    ### ?????? ###
    # ?????????????:?????????,?????????????????, ?????????5??
    set_order_cost(OrderCost(close_tax=0.001, open_commission=0.0003, close_commission=0.0003, min_commission=5), type='stock')

    ## ????(reference_security??????????;???????????,????'000300.XSHG'?'510300.XSHG'????)
      # ?????
    run_daily(before_market_open, time='before_open', reference_security='000300.XSHG')
    # ????
    #run_daily(market_open, time='open', reference_security='000300.XSHG')
    
    run_daily(market_open_sell_all, time='9:30', reference_security='000300.XSHG')
    
      # ?????
    run_daily(after_market_close, time='after_close', reference_security='000300.XSHG')

## ???????
def before_market_open(context):
    # ??????
    log.info('??????(before_market_open):'+str(context.current_dt.time()))
    
    # ???????(??????,???????)
    send_message('?????~')
    
    log.info('????,????api??')
    context.today_stocks_datas = None

    # ??????:????(g.?????)
    #g.security = '000001.XSHE'
    
def handle_data(context,data):
    log.info('??????(handle_data):'+str(context.current_dt.time()))
    #  ????????,????
    if not is_business_time(context):
        return
    stocks_codes = get_api_stocks(context)
    cash = context.portfolio.available_cash
    log.info('handle_data, ??????:'+ str(stocks_codes) + \
                        ', ??????:'+ str(cash))
    if stocks_codes and cash/5 > 10000:
        for stock in stocks_codes:
            if stock not in context.portfolio.positions:
                # ??3???
                rate_3d = compute_last_n_days_rate(context, stock, 3)
                log.info('handle_data,??3???:'+ str(rate_3d))
                if rate_3d > 1.15:
                    log.info('handle_data,??3?????15%,????' +\
                                       '????:'+ stock +\
                                       '3???:' + str(rate_3d))
                else:
                    log.info('handle_data,????,????:'+stock +\
                                        ',????:'+str(cash/5))
                    order_value(stock, cash/5)

## ???????
def market_open_sell_all(context):
    log.info('??????(market_open_sell_all):'+str(context.current_dt.time()))
    for k,v in context.portfolio.positions.items():
        if v.closeable_amount > 0:
            log.info("market_open_sell_all,?????? ????:" + str(k))
            # ??????,????????????0
            order_target(k, 0)
    
## ???????
def after_market_close(context):
    log.info(str('??????(after_market_close):'+str(context.current_dt.time())))
    #??????????
    trades = get_trades()
    for _trade in trades.values():
        log.info('????:'+str(_trade))
        
    log.info('????,????api??')
    context.today_stocks_datas = None
    log.info('????')
    log.info('##############################################################')
    
def get_api_stocks(context):
    diff = datetime.datetime.now().date() - context.current_dt.date()
    stocks = []
    if diff.days > 1:
        log.info('get_api_stocks,???????,API????')
        api_data = context.today_stocks_datas
        if api_data is None:
            api_data = request_quanshow_data_api(context)
            log.info('API?????????:%s'%str(api_data))
            context.today_stocks_datas = api_data
        stocks = filter_request(context,api_data)    
    else:
        if is_business_time(context):
            api_data = request_quanshow_data_api(context)
            stocks = filter_request(context,api_data)
        else:
            log.info('get_api_stocks,????,??????')
            stocks = []
    return stocks

def request_quanshow_data_api(context):
    dat = context.current_dt.strftime('%Y%m%d')
    url = 'https://api.limc.cn/api/Signal'
    params ={"ttl":0,"count":10,"date":dat,'_rnd':random.randint(10000000,99999999)}
    headers = {
        'Content-Type':'application/json; charset=UTF-8',
        'Host':'api.limc.cn',
        'Origin':'https://www.joinquant.com/',
        'Referer':'https://www.joinquant.com/',
        'User-Agent':'Mozilla/5.0 (Linux; Android 6.0;) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Mobile Safari/537.36 QuantShow/0.1.201113',
        'Accept': 'application/json, text/javascript, */*; q=0.01',
    }
    r = requests.get(url,params=params,headers=headers)
    #log.info('API???????:%s'%r.text)
    return r.text
    
def compute_last_n_days_rate(context, stock, days):
    # ???????
    df = attribute_history(stock, days, '1d', ['close','pre_close'])
    quote_rates = ((df.close - df.pre_close) / df.pre_close) + 1.0
    rate_days = 1.0
    # ??
    for rate in quote_rates:
        rate_days = rate_days * rate
    return rate_days

def is_business_time(context):
    tim = context.current_dt.time()
    return (tim.hour == 9) and (tim.minute > 30) and (tim.minute < 50)
    
def filter_request(context,data):
    tim = context.current_dt.time()
    #????
    sbsj_d = tim.hour * 10000 + tim.minute * 100 + tim.second
    sbsj_u = tim.hour * 10000 + (tim.minute + 1) *100 +  tim.second  
    
    df = pd.read_json(data)
    #?? > 90
    #?? > 900
    #?? 3?7
    #??,???,???
    #??1??
    if not df.empty:
        filterd = df[(df.QD >= 90) & \
            (df.TL >= 900) & \
            (df.RATE >= 3.0) & (df.RATE <= 7.0) & \
            (df.CODE > 300000) & (df.CODE < 399000) & \
            (df.SBSJ >= sbsj_d) & (df.SBSJ <= sbsj_u)] 
        stocks_codes = [str(a).zfill(6)+".XSHE" for a in filterd.CODE]
        return stocks_codes;
    return []
    
    