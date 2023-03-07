# -*- encoding=utf8 -*-
import datetime

from sqlalchemy import true

class JobContext(object):
    def __init__(self) -> None:
        self.is_debug = True
        self.current_date = datetime.datetime.now()
        self.today_date = datetime.datetime.today()
        self.last_business_day = self.calc_last_business_day(self.today_date)
        self.is_business_day = self.check_business_day(self.today_date)
        pass

    def check_business_day(self,now_date):
        if datetime.date.weekday(now_date) in [5,6]:
            return False
        else:
            return True

    def calc_last_business_day(self,now_date):
        if datetime.date.weekday(now_date) == 0:      #if it's Monday
            last_business_day = now_date - datetime.timedelta(days = 3) #then make it Friday
        elif datetime.date.weekday(now_date) == 6:      #if it's Sunday
            last_business_day = now_date - datetime.timedelta(days = 2); #then make it Friday
        else:                                            #if it's Tus to Sat
            last_business_day = now_date - datetime.timedelta(days = 1) #then make -1 
        return last_business_day
