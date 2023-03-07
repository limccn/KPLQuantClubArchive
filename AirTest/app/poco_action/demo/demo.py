# -*- encoding=utf8 -*-
__author__ = "limingcheng"

from airtest.core.api import *
from airtest.cli.parser import cli_setup

if not cli_setup():
    auto_setup(__file__, logdir=True, devices=["android://127.0.0.1:5037/103.36.203.193:301?cap_method=MINICAP&&ori_method=MINICAPORI&&touch_method=MAXTOUCH",])


# script content
print("start...")


# generate html report
# from airtest.report.report import simple_report
# simple_report(__file__, logpath=True)


from poco.drivers.android.uiautomation import AndroidUiautomationPoco

poco = AndroidUiautomationPoco(use_airtest_input=True, screenshot_each_action=False)


poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("自选").child("com.hexin.plat.android:id/icon").click()

sleep(1)

poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("交易").child("com.hexin.plat.android:id/icon").click()

sleep(1)

poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("资讯").child("com.hexin.plat.android:id/icon").click()

sleep(1)

poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("首页").child("com.hexin.plat.android:id/icon").click()

sleep(1)
