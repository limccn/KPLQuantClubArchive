# -*- encoding=utf8 -*-

import time
from apscheduler.schedulers.background import BackgroundScheduler
from datetime import datetime
from apscheduler.jobstores.sqlalchemy import SQLAlchemyJobStore
from apscheduler.executors.pool import ThreadPoolExecutor

from app.jobs.ths_scheduler import *

class Scheduler(object):
    def __init__(self) -> None:
        self._scheduler = None
        self._settings = None
        self.setup()

    def setup(self):
        self._settings = {
                #'apscheduler.jobstores.mongo': {
                #    'type': 'mongodb'
                #},
                #'apscheduler.jobstores.default': {
                #    'type': 'sqlalchemy',
                #    'url': 'sqlite:///jobs.sqlite'
                #},
                'apscheduler.executors.default': {
                    'class': 'apscheduler.executors.pool:ThreadPoolExecutor',
                    'max_workers': '20'
                },
                'apscheduler.executors.processpool': {
                    'type': 'processpool',
                    'max_workers': '5'
                },
                'apscheduler.job_defaults.coalesce': 'false',
                'apscheduler.job_defaults.max_instances': '3',
                'apscheduler.timezone': 'UTC',
           }
        #self._scheduler = BlockingScheduler(jobstores=jobstores, executors = executors , job_defaults = job_defaults )
        self._scheduler = BackgroundScheduler(self._settings)

    def init_jobs(self):
        # 
        self._scheduler.add_job(daily_0900_init, trigger="cron" , day_of_week='mon-fri', hour=1, minute=0, second=5,end_date='2023-05-30')
        self._scheduler.add_job(daily_0905_login_account, trigger="cron" , day_of_week='mon-fri', hour=1, minute=5, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_0915_market_open_sell_all, trigger="cron" , day_of_week='mon-fri', hour=1, minute=15, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_0929_check_login_state, trigger="cron" , day_of_week='mon-fri', hour=1, minute=30, second=1, end_date='2023-05-30')
        #self._scheduler.add_job(daily_0925_market_open_sell_all, trigger="cron" , day_of_week='mon-fri', hour=1, minute=27, second=5,end_date='2023-05-30')
        self._scheduler.add_job(daily_interal_1min_0930_to_0959_buy, trigger="cron" , day_of_week='mon-fri', hour=1, minute=30, second=31, end_date='2023-05-30')
        self._scheduler.add_job(daily_interal_1min_0930_to_0959_buy, trigger="cron" , day_of_week='mon-fri', hour=1, minute="31-59/1", second="1,31", end_date='2023-05-30')
        self._scheduler.add_job(daily_1010_cancel_all, trigger="cron" , day_of_week='mon-fri', hour=2, minute=10, second=5, end_date='2023-05-30')
        #self._scheduler.add_job(daily_1030_stop, trigger="cron" , day_of_week='mon-fri', hour=2, minute=30, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_1305_login_account, trigger="cron" , day_of_week='mon-fri', hour=5, minute=5, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_1440_market_open_sell_all, trigger="cron" , day_of_week='mon-fri', hour=5, minute=6, second=40, end_date='2023-05-30')
        
        pass

    def init_jobs2(self):
        # 
        # 收盘或模拟交易
        hour_add = 7
        self._scheduler.add_job(daily_0900_init, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 1, minute=0, second=5,end_date='2023-05-30')
        self._scheduler.add_job(daily_0905_login_account, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 1, minute=10, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_0915_market_open_sell_all, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 1, minute=21, second=5, end_date='2023-05-30')
        #self._scheduler.add_job(daily_0925_market_open_sell_all, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 1, minute=27, second=5,end_date='2023-05-30')
        self._scheduler.add_job(daily_interal_1min_0930_to_0959_buy, trigger="cron" , day_of_week='mon-fri', hour=hour_add +1, minute="30-50/1", second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_1010_cancel_all, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 2, minute=10, second=5, end_date='2023-05-30')
        self._scheduler.add_job(daily_1530_stop, trigger="cron" , day_of_week='mon-fri', hour=hour_add + 2, minute=30, second=5, end_date='2023-05-30')
        

        pass


    def run(self):
        self.init_jobs()
        #self.init_jobs2()
        self._scheduler.start()

        try:
            while True:
                time.sleep(2)
        except (KeyboardInterrupt, SystemExit):
            self._scheduler.shutdown()


scheduler = Scheduler()