# -*- encoding=utf8 -*-
import os
os.environ['RUNNING_STATE'] = 'Production'

from scheduler import scheduler
from app.utils.logger import log


if __name__ == '__main__' :
    try:
        scheduler.run()
    except (KeyboardInterrupt, SystemExit):
        pass