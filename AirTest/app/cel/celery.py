# -*- encoding=utf8 -*-


from __future__ import absolute_import, unicode_literals

import os
os.environ['RUNNING_STATE'] = 'Production'

from celery import Celery



celery_app = Celery('tasks')
#从配置文件启动
celery_app.config_from_object('app.cel.config')

if __name__ == '__main__':
    try:
        celery_app.start()
    except (KeyboardInterrupt, SystemExit):
        pass

