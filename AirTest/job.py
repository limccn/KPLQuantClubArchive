import sys

from app.poco_action.ths_shipan.ths_android import init
sys.path.append('../')

#from app.jobs.job_daily_trader import sell_all_on_market_open

import os
os.environ['RUNNING_STATE'] = 'Production'

import time
from app.cel.tasks import wrap_init


if __name__ == '__main__' :
    try:

        result = wrap_init.delay()
        while not result.ready():
            print(1)
            time.sleep(1)

        #sell_all_on_market_open()
    except (KeyboardInterrupt, SystemExit):
        pass
