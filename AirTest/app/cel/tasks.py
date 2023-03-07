# -*- encoding=utf8 -*-

from __future__ import absolute_import, unicode_literals

import datetime
from app.cel.celery import celery_app

from app.poco_action import order_buy,order_sell,cancel_order,init

def log(msg):
    if isinstance(msg,Exception):
        print("[CELERY][EXCPTION][" + str(datetime.datetime.now()) + "][" + repr(msg) + "]")
        #import traceback
        #print('traceback.format_exc():\n%s' % traceback.format_exc())
    else:
        print("[CELERY][" + str(datetime.datetime.now()) + "]" + str(msg))


@celery_app.task
def add(x, y):
    return x + y

@celery_app.task
def wrap_order_buy(code,price=0.,number=0,amount=0.,percent=0.,maket_price=False,limit_price=False):
    log("Wrapp Celery Method order_buy start")
    order_buy(code,price,number,amount,percent,maket_price,limit_price)
    log("Wrapp Celery Method order_buy end")

@celery_app.task
def wrap_order_sell(code,price=0.,number=0 ,percent=0.,maket_price=False,limit_price=False):
    log("Wrapp Celery Method order_sell start")
    order_sell(code,price,number,percent,maket_price,limit_price)
    log("Wrapp Celery Method order_sell end")
    

@celery_app.task
def wrap_cancel_order(opt="all"):
    log("Wrapp Celery Method cancel_order start")
    cancel_order(opt)
    log("Wrapp Celery Method cancel_order end")


@celery_app.task
def wrap_init():
    log("Wrapp init")
    init()
    log("Wrapp init")
    