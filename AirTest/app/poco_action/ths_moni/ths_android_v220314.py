# -*- encoding=utf8 -*-

from airtest.core.api import *
from airtest.cli.parser import cli_setup


from poco.drivers.android.uiautomation import AndroidUiautomationPoco


_SPEED_ = 1.3

#if not cli_setup():
auto_setup(__file__, logdir=True, devices=["android://127.0.0.1:5037/103.36.203.193:301?cap_method=MINICAP&&ori_method=MINICAPORI&&touch_method=MAXTOUCH",], project_root="D:/limingcheng/kaipanla/trunk/src/AirTest")
    
poco = AndroidUiautomationPoco(use_airtest_input=True, screenshot_each_action=False)

#import requests
#rep = requests.get('https://whatismyip.akamai.com/')
#print(rep.status_code)
    
# script content
#print("start...")


# generate html report
# from airtest.report.report import simple_report
# simple_report(__file__, logpath=True)

def init():
    home()

def stop_app():
    keyevent("BACK")
    sleep(1.0/_SPEED_)
    keyevent("BACK")
    sleep(1.0/_SPEED_)
    keyevent("BACK")
    sleep(1.0/_SPEED_)
    keyevent("BACK")
    sleep(1.0/_SPEED_)
    
def reload_app():
    stop_app()
    start_app("com.hexin.plat.android")

''' =====================STEPS==============='''
def step_startup_app():
    home()
    start_app("com.hexin.plat.android")
    step_clear_popup_message()
    step_clear_navibar_back()

def step_check_account_login(name, account):
    sleep(1.0/_SPEED_)
    poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("交易").child("com.hexin.plat.android:id/icon").click()
    sleep(1.0/_SPEED_)
    # 检查登陆
    if poco("com.hexin.plat.android:id/account_layout").exists() :
        sec_name = poco("com.hexin.plat.android:id/account_layout").offspring("com.hexin.plat.android:id/qs_name_text").get_text()
        sec_account = poco("com.hexin.plat.android:id/account_layout").offspring("com.hexin.plat.android:id/account_text").get_text()
        if name == sec_name and sec_account[-4:] == account:
            return True
    else:
        return False
    
def step_select_and_login_account(name,account,passwd):
    sleep(1.0/_SPEED_)
    poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("交易").child("com.hexin.plat.android:id/icon").click()
    sleep(1.0/_SPEED_)
    # 账户列表
    acc_list = poco("com.hexin.plat.android:id/nobindlist").child("android.widget.LinearLayout")
    if len(acc_list) == 0:
        raise Exception("Can not find Accounts")

    for ele in acc_list:
        sec_name = ele.offspring("com.hexin.plat.android:id/txt_qsname").get_text()
        acc_id = ele.offspring("com.hexin.plat.android:id/txt_account_value").get_text()
        print(acc_id[-4:])
        # 账户名一致
        if sec_name == name and acc_id[-4:] == account:
            ele.click()
            sleep(1.0/_SPEED_)
            poco("android.widget.FrameLayout").offspring("android:id/content").child("com.hexin.plat.android:id/login_component_base_view").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/scrollViewFrame").offspring("com.hexin.plat.android:id/weituo_edit_trade_password").set_text(str(passwd))
            sleep(1.0/_SPEED_)
            poco("android.widget.FrameLayout").offspring("android:id/content").child("com.hexin.plat.android:id/login_component_base_view").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/scrollViewFrame").offspring("com.hexin.plat.android:id/weituo_btn_login").click()
            # 找到账户以后正常返回
            return
        else:
            pass
    raise Exception("Can not find Login Account")

def step_clear_popup_message():
    #sleep(1.0/_SPEED_)
    # 确定类消息
    popup_btn = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/ok_btn")
    popup_title = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_title")
    if popup_btn.exists() and popup_title.exists():
        popup_btn.click()
        sleep(1.0/_SPEED_)

    # 新股申购类消息
    popup_cancel = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/option_cancel")
    #popup_setting = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/option_setting")
    popup_apply = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/option_apply")

    if popup_cancel.exists() and popup_apply.exists():
        popup_cancel.click()
        sleep(1.0/_SPEED_)

def step_clear_navibar_back():
    #sleep(1.0/_SPEED_)
    has_navibar = poco("com.hexin.plat.android:id/page_title_bar").offspring("com.hexin.plat.android:id/title_bar_left_container").offspring("com.hexin.plat.android:id/title_bar_img")
    if has_navibar.exists():
        has_navibar.click()
        sleep(1.0/_SPEED_)


def step_jump_to_trade_tab():
    sleep(1.0/_SPEED_)
    # 实盘跳到
    # 点击交易按钮
    btn_trade=poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("交易").child("com.hexin.plat.android:id/icon")
    if btn_trade.exists():
        btn_trade.click()
        sleep(1.0/_SPEED_)  
        #选择A股交易
        poco("com.hexin.plat.android:id/title_bar_middle_container").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/tab_a").click()
    else:
        raise Exception("Can not find trade tab")

def step_sim_jump_to_trade_tab():
    sleep(1.0/_SPEED_)
    # 点击交易按钮
    btn_trade=poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("交易").child("com.hexin.plat.android:id/icon")
    if btn_trade.exists():
        btn_trade.click()
        sleep(1.0/_SPEED_)  
        #选择模拟交易
        #poco("com.hexin.plat.android:id/tab_mn").click()
        poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/tab_mn").click()
    else:
        raise Exception("Can not find trade tab")


def step_select_menu_buy():
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/capital_layout3").child("com.hexin.plat.android:id/menu_buy").click()
    sleep(1.0/_SPEED_)
    navibuton = poco("com.hexin.plat.android:id/navi_buttonbar").child("android.widget.RelativeLayout")[0].child("com.hexin.plat.android:id/btn")
    #navibuton = poco("com.hexin.plat.android:id/navi_buttonbar").offspring("com.hexin.plat.android:id/btn", desc=u"买入")
    if navibuton.exists():
        navibuton.click()
    else:
        raise Exception("Can not find buy navibar")
    
def step_select_menu_sell():
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/capital_layout3").child("com.hexin.plat.android:id/menu_sale").click()
    sleep(1.0/_SPEED_)
    navibuton = poco("com.hexin.plat.android:id/navi_buttonbar").child("android.widget.RelativeLayout")[1].child("com.hexin.plat.android:id/btn")
    if navibuton.exists():
        navibuton.click()
    else:
        raise Exception("Can not find sell navibar")

def step_select_menu_cancel():
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/capital_layout3").child("com.hexin.plat.android:id/menu_withdrawal").click()
    sleep(1.0/_SPEED_)
    navibuton = poco("com.hexin.plat.android:id/navi_buttonbar").child("android.widget.RelativeLayout")[2].child("com.hexin.plat.android:id/btn")
    if navibuton.exists():
        navibuton.click()
    else:
        raise Exception("Can not find cancel navibar")

def step_input_stock_code(code):
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/auto_stockcode").click()
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/dialogplus_view_container").offspring("com.hexin.plat.android:id/content_stock").child("com.hexin.plat.android:id/auto_stockcode").child("com.hexin.plat.android:id/auto_stockcode").click()
    sleep(1.0/_SPEED_)
    poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("android:id/content").offspring("com.hexin.plat.android:id/dialogplus_content_container").offspring("com.hexin.plat.android:id/dialogplus_view_container").offspring("com.hexin.plat.android:id/content_stock").child("com.hexin.plat.android:id/auto_stockcode").child("com.hexin.plat.android:id/auto_stockcode").set_text(str(code))
    sleep(1.0/_SPEED_)
    #只取第一个,检查代码是否成功
    select_item = poco("com.hexin.plat.android:id/dialogplus_content_container").offspring("com.hexin.plat.android:id/stockcode_tv")
    if select_item.get_text().strip(" ") == code:
        select_item.click()
    else:
        raise Exception("Stock code is not match")

def step_input_stock_buy_price(price, maket_price=False,limit_price=False):
    order_price = price
    sleep(1.0/_SPEED_)
    
    # 市场价则按
    if maket_price:
        str_current = poco("com.hexin.plat.android:id/layout_stockprice").child("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").get_text()
        order_price = "%.2f"%(float(str_current) * 1.011)
    elif limit_price:         
        ele_limit = poco("com.hexin.plat.android:id/zhangting").offspring("com.hexin.plat.android:id/zhangting_layout").offspring("com.hexin.plat.android:id/zhangtingprice")
        order_price = ele_limit.get_text()
    elif price >0:
        order_price = str(price)
    else:
        order_price = poco("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").get_text()

    # 输入交易价格
    poco("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").set_text(order_price)
    return float(order_price)

def step_input_stock_sell_price(price, maket_price=False,limit_price=False):
    order_price = price
    sleep(1.0/_SPEED_)
    
    # 市场价则按买五价格卖出
    if maket_price:
        str_current = poco("com.hexin.plat.android:id/layout_stockprice").child("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").get_text()
        order_price = "%.2f"%(float(str_current) * 0.989)
    elif limit_price:
        ele_limit = poco("com.hexin.plat.android:id/dieting_layout").child("com.hexin.plat.android:id/dietingprice")
        order_price = ele_limit.get_text()
    elif price >0:
        order_price = str(price)
    else:
        order_price = poco("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").get_text()
        
    # 输入交易价格
    poco("com.hexin.plat.android:id/stockprice").offspring("android.widget.EditText").set_text(order_price)
    return float(order_price)

def step_input_stock_count(number):
    order_number = number
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/stockvolume").offspring("android.widget.EditText").set_text(str(order_number))
    return number

def step_input_stock_count_by_hold(per):
    #活动可买数量
    couldbuy = poco("android.widget.FrameLayout").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/page_content").child("android.widget.LinearLayout").offspring("com.hexin.plat.android:id/couldbuy").get_text()
    # 没有可买uu
    if couldbuy ==u"可买0股":
        raise Exception("No available cash balance to buy")
    if couldbuy ==u"可卖0股":
        raise Exception("No available stock hold to sell")
    
    if per == 1:
        poco("com.hexin.plat.android:id/all_chicang").click()
    elif per == 0.5:
        poco("com.hexin.plat.android:id/half_chicang").click()
    elif per == 0.33:
        poco("com.hexin.plat.android:id/one_third_chicang").click()
    elif per == 0.25:
        poco("com.hexin.plat.android:id/one_four_chicang").click()
    else:
        raise Exception("No valid chicang percent")

def step_tap_pre_excute():
    sleep(1.0/_SPEED_)
    poco("com.hexin.plat.android:id/button_container").offspring("android.widget.TextView").click()

def step_after_finish_order():
    sleep(1.0/_SPEED_)
    result_promote = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/prompt_content")
    if result_promote.exists():
        result_text = poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/prompt_content").get_text()
        # 判断下单结果
        if result_text == u"现在已闭市,系统禁止委托":
            pass
        if result_text == u'托已提交':
            pass
    else:
        raise Exception("Invalida opration")
    # 确定
    if poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/ok_btn").exists():
        poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/ok_btn").click()

 
def login_account(name, account, passwd):
    call_result = False
    try:
        step_startup_app()
        is_login = step_check_account_login(name, account)
        if not is_login:
            step_select_and_login_account(name, account, passwd)
        call_result = True
    except Exception as e:
        print(e)
        call_result = False
    finally:
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        return call_result



def order_buy(code,price=0.,number=0,amount=0.,percent=0.,maket_price=False,limit_price=False):
    call_result = False
    try:
        step_startup_app()
        # 模拟交易
        step_sim_jump_to_trade_tab()
        #step_jump_to_trade_tab()
        
        step_select_menu_buy()
        step_input_stock_code(code)
        order_price = step_input_stock_buy_price(price, maket_price, limit_price)

        if number > 0:
            step_input_stock_count(number)
        elif percent > 0:
            step_input_stock_count_by_hold(percent)
        elif amount > 0:
            number = int(amount/100。/order_price) * 100
            if number > 0:
                step_input_stock_count(number)
            else:
                raise Exception("Invalid Amount")
        else:
            raise Exception("Invalid number parameters")

        # 点击按钮，交易前确认
        step_tap_pre_excute()

        order_check = True
        order_check_msg = ""

        # 检查下单方向
        chk_excute_text = poco("com.hexin.plat.android:id/ok_btn").get_text()
        if chk_excute_text != u'确认买入':
            order_check_msg = "Invalid opertaion"
            order_check = False

        # 检查下单
        chk_code = poco("com.hexin.plat.android:id/stock_code_value").get_text()
        if chk_code.strip(" ") != code:
            order_check_msg = "Invalid stock code"
            order_check = False

        chk_number = poco("com.hexin.plat.android:id/number_value").get_text()
        if number > 0 and (float(chk_number) - number) !=0 :
            order_check_msg = "Invalid stock number"
            order_check = False

        chk_price = poco("com.hexin.plat.android:id/price_value").get_text()
        if price > 0  and (float(chk_price) - price) != 0:
            order_check_msg = "Invalid stock price"
            order_check = False
        elif (float(chk_price) - order_price) != 0:
            order_check_msg = "Invalid stock order price"
            order_check = False
        else:
            pass    
    
        # 检查输入输出，下单
        # 检查输入输出，下单
        if order_check:
            #poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/ok_btn").click()
            poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_layout").offspring("com.hexin.plat.android:id/ok_btn").click()
        else:
            #poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/cancel_btn").click()
            poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_layout").offspring("com.hexin.plat.android:id/cancel_btn").click()
            raise Exception(order_check_msg)

        #执行后续操作
        step_after_finish_order()
        call_result = True
    except Exception as e:
        print(e)
        call_result = False
    finally:
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        return call_result

def order_sell(code,price=0.,number=0 ,percent=0.,maket_price=False,limit_price=False):
    call_result = False
    try:
        step_startup_app()
        # 模拟交易
        step_sim_jump_to_trade_tab()
        #step_jump_to_trade_tab()
        
        step_select_menu_sell()
        step_input_stock_code(code)
        
        order_price = step_input_stock_sell_price(price, maket_price, limit_price)

        
        if number > 0:
            step_input_stock_count(number)
        elif percent > 0:
            step_input_stock_count_by_hold(percent)
        else:
            raise Exception("Invalid number parameters")

        # 点击按钮，交易前确认
        step_tap_pre_excute()
        
        order_check = True
        order_check_msg = ""

        chk_excute_text = poco("com.hexin.plat.android:id/ok_btn").get_text()
        if chk_excute_text != u'确认卖出':
            raise Exception("Invalid opertaion")

        # 检查下单股票
        chk_code = poco("com.hexin.plat.android:id/stock_code_value").get_text()
        if chk_code.strip(" ") != code:
            order_check_msg = "Invalid stock code"
            order_check = False

        chk_number = poco("com.hexin.plat.android:id/number_value").get_text()
        if number > 0 and (float(chk_number) - number) !=0 :
            order_check_msg = "Invalid stock number"
            order_check = False

        chk_price = poco("com.hexin.plat.android:id/price_value").get_text()
        if price > 0  and (float(chk_price) - price) != 0:
            order_check_msg = "Invalid stock price"
            order_check = False
        elif (float(chk_price) - order_price) != 0:
            order_check_msg = "Invalid stock order price"
            order_check = False
        else:
            pass
    
        # 检查输入输出，下单
        if order_check:
            #poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/ok_btn").click()
            poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_layout").offspring("com.hexin.plat.android:id/ok_btn").click()
        else:
            #poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/cancel_btn").click()
            poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_layout").offspring("com.hexin.plat.android:id/cancel_btn").click()
            raise Exception(order_check_msg)


        #执行后续操作
        step_after_finish_order()
        call_result = True
    except Exception as e:
        print(e)
        call_result = False
    finally:
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        return call_result
    
def cancel_order(opt="all"):
    call_result = True
    try:
        step_startup_app()
        # 模拟交易
        step_sim_jump_to_trade_tab()
        #step_jump_to_trade_tab()
        
        step_select_menu_cancel()
        
        sleep(1.0/_SPEED_)
        menu = poco("com.hexin.plat.android:id/gdqc_layout")
        if not menu.exists():
            raise Exception("No Order to cancel")

        if opt == "all":
            menu.offspring("com.hexin.plat.android:id/quanche_tv").click()
        elif opt == "buy":
            menu.offspring("com.hexin.plat.android:id/che_buy_tv").click()
        elif opt == "sell":
            menu.offspring("com.hexin.plat.android:id/che_sell_tv").click()         
        else:
            pass   
        sleep(1.0/_SPEED_)

        # 检查输入输出，撤单
        poco("android.widget.FrameLayout").offspring("com.hexin.plat.android:id/dialog_layout").offspring("com.hexin.plat.android:id/ok_btn").click()

        call_result = True
    except Exception as e:
        print(e)
        call_result = False
    finally:
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        sleep(1.0/_SPEED_)
        keyevent("BACK")
        return call_result