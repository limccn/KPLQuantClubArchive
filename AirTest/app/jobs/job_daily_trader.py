# -*- encoding=utf8 -*-

import datetime

import random
import numpy as np
import pandas as pd

from app.poco_action.ths_shipan.ths_android import check_login_state

from .job_context import JobContext

import requests

from app.utils.logger import log

from app.poco_action import order_buy, order_sell, init, cancel_order, login_account, stop_app

def request_quanshow_data_api(context):
    dat = context.current_date.strftime('%Y%m%d')

    url = 'https://api.limc.cn/api/Portfolio'
    params ={"ttl":0, "count":20,"date":dat,'_rnd':random.randint(10000000,99999999)}
    headers = {
        'Content-Type':'application/json; charset=UTF-8',
        'Host':'api.limc.cn',
        'Origin':'https://api.limc.cn/',
        'Referer':'https://api.limc.cn/',
        'User-Agent':'Mozilla/5.0 (Linux; Android 6.0;) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Mobile Safari/537.36 QuantShow/0.1.201113',
        'Accept': 'application/json, text/javascript, */*; q=0.01',
    }
    log.info('[API]Sending Request @'+str(datetime.datetime.now()))
    r = requests.get(url,params=params,headers=headers)
    log.info('[API]Receive Response, @'+str(datetime.datetime.now()))
    return r.text


def request_previous_quantshow_data_api(context):
    dat = context.last_business_day.strftime('%Y%m%d')

    url = 'https://api.limc.cn/api/PortfolioSignal'
    params ={"ttl":0, "count":20,"date":dat,'_rnd':random.randint(10000000,99999999)}
    headers = {
        'Content-Type':'application/json; charset=UTF-8',
        'Host':'api.limc.cn',
        'Origin':'https://api.limc.cn/',
        'Referer':'https://api.limc.cn/',
        'User-Agent':'Mozilla/5.0 (Linux; Android 6.0;) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Mobile Safari/537.36 QuantShow/0.1.201113',
        'Accept': 'application/json, text/javascript, */*; q=0.01',
    }
    log.info('[API]Sending Request @'+str(datetime.datetime.now()))
    r = requests.get(url,params=params,headers=headers)
    log.info('[API]Receive Response, @'+str(datetime.datetime.now()))
    return r.text


def is_business_time(context):
    ''' 9:30???9???59 ??????????????????????????????, ???????????????50???''' 
    tim = context.current_date.time()
    return (tim.hour == 9) and (tim.minute >= 30) and (tim.minute <= 59)

def is_periodic_time(context):
    ''' 9:15???9???30 ?????????????????????, ???????????????29???''' 
    tim = context.current_date.time()
    return (tim.hour == 9) and (tim.minute >= 15) and (tim.minute < 30)
    
def is_before_close_time(context):
    ''' 14:45 ????????????????????????????????????''' 
    tim = context.current_date.time()
    if(tim.hour < 14):
        return True
    else:
        return tim.minute <= 45
    
def filter_alorithm_2(context, df):
    tim = context.current_date
    # 30???????????????,??????29??????????????????
    if tim.hour == 9 and tim.minute == 30:
        xhqrsj_d = 92901
        xhqrsj_u = 93030
    else:
        if tim.second >= 30:
            #??????????????????, ????????????10s???????????????, 093004???093103
            xhqrsj_d = tim.hour * 10000 + (tim.minute - 1)  * 100  + 59
            xhqrsj_u = tim.hour * 10000 + tim.minute * 100 + 28
        else:
            #??????????????????, ????????????10s???????????????, 093004???093103
            xhqrsj_d = tim.hour * 10000 + (tim.minute - 1) * 100 + 29
            xhqrsj_u = tim.hour * 10000 + (tim.minute - 1) * 100 + 58

    #log.info('filter_alorithm_2, ???????????????????????????'+str(xhqrsj_d) + '??? ??? ???' + str(xhqrsj_u)+'???')
    #??????  ?????????
    #??????,?????????, ??????????????????
    #??????1??????
    #????????????????????????
    if not context.is_debug:
        fi = df[(df.CODE > 0) & (df.CODE < 688000) &\
                (df.XHQRSJ >= xhqrsj_d) & (df.XHQRSJ <= xhqrsj_u)] 
        #fi = df[(df.CODE > 0) & (df.CODE < 688000) & \
        #        (df.XHQRSJ >= xhqrsj_d) & (df.XHQRSJ <= xhqrsj_u)] 
    else:
        fi = df[(df.CODE > 0) & (df.CODE < 688000)]   
            
    return fi

def filter_alorithm_3(context, df):
    # ??????9:29???9:59 ??????????????????,??????1??????
    xhqrsj_d = 92900
    xhqrsj_u = 95959
    #??????  ?????????
    #??????,?????????, ??????????????????
    #??????
    fi = df[(df.CODE > 0) & (df.CODE < 688000) &\
            (df.XHQRSJ >= xhqrsj_d) & (df.XHQRSJ <= xhqrsj_u) &\
            (df.TAG != "????????????") & (df.TAG != "????????????")]   
            
    return fi

def filter_alorithm_4(context, df):
    # ??????9:29???9:59 ??????????????????,??????1??????
    xhqrsj_d = 92900
    xhqrsj_u = 95959
    #??????  ?????????
    #??????,?????????, ??????????????????
    #??????
    fi = df[(df.CODE > 0) & (df.CODE < 688000) &\
            (df.XHQRSJ >= xhqrsj_d) & (df.XHQRSJ <= xhqrsj_u) &\
            (df.TAG == "????????????")]   
            
    return fi

def filter_request(context,data):
    df = pd.read_json(data)
    if not df.empty:
        # ????????????2 ??????
        filtered_fi = filter_alorithm_2(context, df)
        #??????append??????????????????
        #all_fi.drop_duplicates(['CODE'],keep="last")
            
        stocks_codes = [reformat_stock_code(a) for a in filtered_fi.CODE]
        return stocks_codes
    return []

def filter_previous_request(context,data,market_close=False):
    df = pd.read_json(data)
    if not df.empty:
        # ???????????????????????????????????????
        if market_close:
            # ???????????????
            filtered_fi = filter_alorithm_4(context, df) 
        else:
            #????????????
            filtered_fi = filter_alorithm_3(context, df) 
        stocks_codes = [reformat_stock_code(a) for a in filtered_fi.CODE]
        return stocks_codes
    return []
    

def get_api_stocks(context):
    stocks = []
    if is_business_time(context) or context.is_debug:
        api_data = request_quanshow_data_api(context)
        stocks = filter_request(context,api_data)
    else:
        log.info('get_api_stocks, not in business time ')
        stocks = []
    return stocks


def get_api_previous_stocks(context,market_close=False):
    stocks = []
    if is_periodic_time(context) or context.is_debug:
        api_data = request_previous_quantshow_data_api(context)
        stocks = filter_previous_request(context,api_data, market_close)
    else:
        log.info('get_api_previous_stocks ')
        stocks = []
    return stocks


def reformat_stock_code(code):
    str_code = str(code).zfill(6)
    if str_code[0:2] in ['60','68','90'] :
        return str_code + ''
    if str_code[0:2] in ['00','30','20'] :
        return str_code + ''

def initialize():
    log.info('[THX] =====initialize======, @'+str(datetime.datetime.now()))
    init()
    log.info('[THX] =====initialize======, @'+str(datetime.datetime.now()))

def init_context():
    context = JobContext()
    # ????????????????????????????????????
    context.is_debug = False
    return context

def daily_market_open():
    log.info('[THX] =====daily_market_open======, @'+str(datetime.datetime.now()))
    # ????????????????????????
    login_account("????????????","5113","100442") 

    log.info('[THX] =====daily_market_open======, @'+str(datetime.datetime.now()))

def daily_market_periodic_time():
    log.info('[THX] =====daily_market_periodic_time======, @'+str(datetime.datetime.now()))
    # ????????????????????????
    login_account("????????????","5113","100442") 
    # ??????????????????
    log.info('[THX] =====daily_market_periodic_time======, @'+str(datetime.datetime.now()))


def daily_market_period_check_login():
    log.info('[THX] =====daily_market_periodic_time======, @'+str(datetime.datetime.now()))
    # ????????????????????????
    check_login_state("????????????","5113") 
    # ??????????????????
    log.info('[THX] =====daily_market_periodic_time======, @'+str(datetime.datetime.now()))


def handle_data():
    log.info('[THX] =====handle_data======, @'+str(datetime.datetime.now()))
    context = init_context()
    # ??????????????????????????????
    if not context.is_business_day:
        log.info('[THX] Today is not bussiness data Cancel, @'+str(datetime.datetime.now()))
        return
    stocks = get_api_stocks(context)
    for stock in stocks:
        log.info('[THX] Begin Poco Order, @'+str(datetime.datetime.now()))
        # ????????????????????????????????????
        if stock[0:2] in ['30','68']:
            log.info("Stock 300XXX, Cancel :" + str(stock) )
            return
        log.info("[THX] Buy Order code:" + str(stock) + "amount = 10000 , use Market Price")
        order_result = order_buy(code=stock, amount=10000, maket_price=True)
        if order_result :
            log.info('[THX] Buy Order Success, @'+str(datetime.datetime.now()))
        else:
            log.info('[THX] Buy Order Failure, @'+str(datetime.datetime.now()))
        log.info('[THX] Finish Poco Order, @'+str(datetime.datetime.now()))
    log.info('[THX] =====handle_data======, @'+str(datetime.datetime.now()))


def daily_market_open_sell_all():
    log.info('[THX] =====sell_all_on_market_open======, @'+str(datetime.datetime.now()))
    context = init_context()
    # ??????????????????????????????
    if not context.is_business_day:
        log.info('[THX] Today is not bussiness data Cancel, @'+str(datetime.datetime.now()))
        return
    stocks = get_api_previous_stocks(context, market_close=False)
    for stock in stocks:
        log.info('[THX] Begin Poco Sell Order, @'+str(datetime.datetime.now()))
        log.info("[THX] Sell Order code:" + str(stock) + "use Limit Price sell ALL")
        order_result = order_sell(code=stock, percent=1.0, limit_price=True, cancel_ifis_upper=False)
        if order_result :
            log.info('[THX] Sell Order Success, @'+str(datetime.datetime.now()))
        else:
            log.info('[THX] Sell Order Failure, @'+str(datetime.datetime.now()))
        log.info('[THX] Sell Finish Poco Order, @'+str(datetime.datetime.now()))
    log.info('[THX] =====sell_all_on_market_open======, @'+str(datetime.datetime.now()))


def daily_market_will_close_sell_all():
    log.info('[THX] =====sell_all_on_market_will_close======, @'+str(datetime.datetime.now()))
    context = init_context()
    # ??????????????????????????????
    if not context.is_business_day:
        log.info('[THX] Today is not bussiness data Cancel, @'+str(datetime.datetime.now()))
        return
    stocks = get_api_previous_stocks(context,market_close=True)
    for stock in stocks:
        log.info('[THX] Begin Poco Sell Order, @'+str(datetime.datetime.now()))
        log.info("[THX] Sell Order code:" + str(stock) + "use Limit Price sell ALL, Cancel if is upper limit")
        order_result = order_sell(code=stock, percent=1.0, limit_price=True, cancel_ifis_upper=True)
        if order_result :
            log.info('[THX] Sell Order Success, @'+str(datetime.datetime.now()))
        else:
            log.info('[THX] Sell Order Failure, @'+str(datetime.datetime.now()))
        log.info('[THX] Sell Finish Poco Order, @'+str(datetime.datetime.now()))
    log.info('[THX] =====sell_all_on_market_will_close======, @'+str(datetime.datetime.now()))

def daily_cancel_all_order():
    log.info('[THX] =====cancel_all_order======, @'+str(datetime.datetime.now()))
    # ??????????????????
    cancel_order(opt="all")
    log.info('[THX] =====cancel_all_order======, @'+str(datetime.datetime.now()))

def daily_market_close():
    log.info('[THX] =====daily_market_close======, @'+str(datetime.datetime.now()))
    # ??????app
    stop_app()
    log.info('[THX] =====daily_market_close======, @'+str(datetime.datetime.now()))

    

