# -*- encoding=utf8 -*-

from app.utils.logger import log
from app.jobs.job_daily_trader import daily_market_will_close_sell_all, initialize,daily_market_open,daily_market_period_check_login,daily_market_periodic_time,daily_market_open_sell_all,daily_cancel_all_order,handle_data,daily_market_close

def daily_0900_init():
    log.info("[Schedual]09:00 Init")
    initialize()

def daily_0905_login_account():
    log.info("[Schedual]09:05 Check Account Login")
    daily_market_open()

def daily_1305_login_account():
    log.info("[Schedual]09:10 Periodic Time Login")
    daily_market_periodic_time()    

def daily_0915_market_open_sell_all():
    log.info("[Schedual]09:21 market open sell all")
    daily_market_open_sell_all()
    pass

def daily_0925_market_open_sell_all():
    log.info("[Schedual]09:25 market open sell all")
    daily_market_open_sell_all()
    pass

def daily_0929_check_login_state():
    log.info("[Schedual]09:29 check login state")
    daily_market_period_check_login()
    pass

def daily_1010_cancel_all():
    log.info("[Schedual]10:10 Cancel all")
    daily_cancel_all_order()
    pass

def daily_1440_market_open_sell_all():
    log.info("[Schedual]14:40 market close sell all")
    daily_market_will_close_sell_all()
    pass

def daily_interal_1min_0930_to_0959_buy():
    log.info("[Schedual] 09:30 to 09:59 Interal Trigger 1min")
    handle_data()
    pass

def daily_1530_stop():
    log.info("[Schedual]11:00 Stop APP")
    daily_market_close()


