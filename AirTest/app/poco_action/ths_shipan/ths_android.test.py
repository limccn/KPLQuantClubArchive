# -*- encoding=utf8 -*-
import os
os.environ['RUNNING_STATE'] = 'Production'

from ths_android import *
from airtest.core.api import *



_SPEED_ = 1.0

#login_account("东兴证券","5113","100442")

#init()
#check_account_login("华鑫证券","4115","871642")
#check_account_login("东兴证券","5113","100442")


#select_account("华鑫证券","4115","871642")
#order_buy("601288",2.7,100)
#order_buy_percent("601288",2.7,0.25)
#order_sell_percent("000731",500,0.25)
#sleep(1/_SPEED_)
#order_buy(code ="601288", price = 2.7, number=1000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", price = 2.7, percent=0.25)
#sleep(1/_SPEED_)
#order_buy(code = "601288", price = 2.7, amount=10000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", limit_price=True, amount=10000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", maket_price=True, amount=10000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", maket_price=True, number=1000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", limit_price=True, number=1000)
#sleep(1/_SPEED_)
#order_buy(code = "601288", maket_price=True, percent=0.25)
#sleep(1/_SPEED_)
#order_buy(code = "601288", limit_price=True, percent=0.25)

#sleep(1/_SPEED_)
#order_sell(code ="601288", price = 3.0, number=100)
#sleep(1/_SPEED_)
#order_sell(code = "601288", price = 3.0, percent=0.25)
#sleep(1/_SPEED_)
#order_sell(code = "601288", maket_price=True, number=100)
#sleep(1/_SPEED_)
#order_sell(code = "601288", limit_price=True, number=100)
#sleep(1/_SPEED_)
#order_sell(code = "601288", maket_price=True, percent=0.25)
#sleep(1/_SPEED_)
#order_sell(code = "601288", limit_price=True, percent=0.25)

#order_sell(code = "601288", limit_price=True, percent=1)

import datetime

sleep(1/_SPEED_)
init()

#sleep(1/_SPEED_)
#print("============login_account start=========@" + str(datetime.datetime.now()))
#login_account("东兴证券","5113","100442")
#print("============login_account start=========@" + str(datetime.datetime.now()))

#sleep(1/_SPEED_)
#print("============order_buy start=========@" + str(datetime.datetime.now()))
#order_sell(code = "002183", limit_price=True, percent=0.25, cancel_ifis_upper=True)
#print("============order_buy end=========@" + str(datetime.datetime.now()))

#exit()


sleep(1/_SPEED_)
print("============order_buy start=========@" + str(datetime.datetime.now()))
order_buy(code = "601288", limit_price=True, percent=0.25)
print("============order_buy end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_buy start=========@" + str(datetime.datetime.now()))
order_buy(code = "601288", limit_price=True, percent=0.25)
print("============order_buy end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_sell start=========@" + str(datetime.datetime.now()))
order_sell(code = "601288", limit_price=True, percent=0.25)
print("============order_sell end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_sell end=========@" + str(datetime.datetime.now()))
order_sell(code = "601288", limit_price=True, percent=0.25)
print("============order_sell end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_buy end=========@" + str(datetime.datetime.now()))
cancel_order(opt="buy")
print("============order_buy end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============cancel_order end=========@" + str(datetime.datetime.now()))
cancel_order(opt="sell")
print("============cancel_order end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_buy end=========@" + str(datetime.datetime.now()))
order_buy(code = "601288", limit_price=True, percent=0.25)
print("============order_buy end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============order_sell end=========@" + str(datetime.datetime.now()))
order_sell(code = "601288", limit_price=True, percent=0.25)
print("============order_sell end=========@" + str(datetime.datetime.now()))

sleep(1/_SPEED_)
print("============cancel_order start=========@" + str(datetime.datetime.now()))
cancel_order()
print("============cancel_order end=========@" + str(datetime.datetime.now()))



#sleep(1/_SPEED_)
#init()
#sleep(1/_SPEED_)
#login_account("东兴证券","5113","100442")
#sleep(5/_SPEED_)
#login_account("华鑫证券","4115","871642")
#sleep(1/_SPEED_)
