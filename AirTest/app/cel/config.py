# -*- encoding=utf8 -*-

from celery.app.utils import _TO_NEW_KEY
# 
#broker
BROKER_URL = 'redis://localhost:6379/0'
#backen
CELERY_RESULT_BACKEND = 'redis://localhost:6379/1'
#导入任务
CELERY_IMPORTS = ('app.cel.tasks', )
#列化任务载荷的默认的序列化方式
CELERY_TASK_SERIALIZER = 'json'
#结果序列化方式
CELERY_RESULT_SERIALIZER = 'json'
# 
CELERY_ACCEPT_CONTENT=['json']
#时间地区与形式
CELERY_TIMEZONE = 'Asia/Shanghai'
#时间是否使用utc形式
CELERY_ENABLE_UTC = True

#设置任务的优先级或任务每分钟最多执行次数
CELERY_ROUTES = {
    # 如果设置了低优先级，则可能很久都没结果
    #'tasks.add': 'low-priority',
    #'tasks.add': {'rate_limit': '10/m'}，
    #'tasks.add': {'rate_limit': '10/s'}，
    '*': {'rate_limit': '10/s'}
}
#borker池，默认是10
BROKER_POOL_LIMIT = 10
#任务过期时间，单位为s，默认为一天
CELERY_TASK_RESULT_EXPIRES = 3600
#backen缓存结果的数目，默认5000
CELERY_MAX_CACHED_RESULTS = 10000
