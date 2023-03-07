//index.js
//获取应用实例
const app = getApp()

Page({
  /**
   * 页面的初始数据
   */
  data: {
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    title: '加载中...', // 状态
    list: [], // 数据列表
    loading: true, // 显示等待框
    valid_date: '',
    update_time: '', // 数据更新时间
    has_msg: false, // 包含消息
    msg: '',
    date: '',
    rightshowflag: false,
    task: "",
    num: "",
    numflag: false,
    valid_date_flag: false,
    ui_config:{
      ui_title:"普洱行情网",
      ui_loading:"加载中....",
      ui_login_now:"立即登录",
      ui_expire:"订阅有效期：",
      ui_strength:"当前价格",
      ui_rate:"涨跌幅",
      ui_description:"商品说明",
      ui_turnover:"昨日价格",
      ui_buy:"前日价格",
      ui_sell:"相比价差",
      ui_symbol_name:"系列名称",
      ui_hello:"您好,",
      ui_greeting:"您的账户已经过期或未登录，如果你想继续使用狙击龙头，请先点击左上角的立即登录按钮或联系客服人员（微信号：ringenrou）",
    },
    wxapp_settings:{
      config_appkey : '', 
      config_appsecret : '', 
      settings_url : 'https://api.limc.cn/api/ui/settings_666.json',
      data_api_url : 'https://api.limc.cn/api/Portfolio',
      data_api_param_count : '5',
      data_api_param_ttl : '0', 
      user_info_url: 'https://api.limc.cn/api/UserInfo',
      user_sub_url : 'https://api.limc.cn/api/Subscribe',
    },
  },

  getDateToString: function (date) {
    var date_ = date ? date : new Date();
    var year = date_.getFullYear();
    var month = date_.getMonth() + 1;
    var day = date_.getDate();
    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;

    return year + "-" + month + "-" + day;
  },

  getDateTimeToString: function (date) {
    var date_ = date ? date : new Date();
    var year = date_.getFullYear();
    var month = date_.getMonth() + 1;
    var day = date_.getDate();
    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;

    var hours = date_.getHours();
    var mins = date_.getMinutes();
    var secs = date_.getSeconds();
    var msecs = date_.getMilliseconds();
    if (hours < 10) hours = "0" + hours;
    if (mins < 10) mins = "0" + mins;
    if (secs < 10) secs = "0" + secs;
    if (msecs < 10) secs = "0" + msecs;
    return year + "-" + month + "-" + day + " " + hours + ":" + mins + ":" + secs;
  },
  parseData: function (data) {
    var result = new Array();
    for (var i = 0; i < data.length; i++) {
      var item = data[i];
      result[i] = {
        RN: item.RN,
        CODE: item.CODE,
        NAME: item.NAME,
        SBSJ: item.XHQRSJ.replace(/(\d{2})(\d{2})(\d{2})/mg, '$1:$2'),
        QD: 100, 
        TL: 100,
        TUDE: item.TUDE,
        SJLTP: Math.floor(item.SJLTP / 100000000).toString() + "亿",
        ZLJE: Math.floor(item.ZLJE / 10000).toString() + "万",
        BUY: Math.floor(item.BUY / 10000).toString() + "万",
        SELL: Math.floor(item.SELL / 10000).toString() + "万",
        RATE: item.RATE + "%",
        //ZX_ZLJE: Math.floor(item.ZX_ZLJE / 10000).toString() + "万",
        //ZX_BUY: Math.floor(item.ZX_BUY / 10000).toString() + "万",
        //ZX_SELL: Math.floor(item.ZX_SELL / 10000).toString() + "万",
      };
    }
    return result;
  },

  requestData: function () {
    const _this = this;
    // 拼接请求url
    var url = _this.data.wxapp_settings.data_api_url + '?' + 
        '_rnd=' + Math.random()+
        '&ttl='+ _this.data.wxapp_settings.data_api_param_ttl + 
        '&count='+ _this.data.wxapp_settings.data_api_param_count + 
        '&appkey='+ _this.data.wxapp_settings.config_appkey + 
        '&appsecret='+ _this.data.wxapp_settings.config_appsecret;
    if (_this.data.date != '' && _this.data.date != null) {
      url = url + "&date=" + _this.data.date.replace(new RegExp('-', 'g'), '');
    } else {
      url = url
    }
    // 请求数据
    wx.request({
      url: url,
      data: {},
      header: {
        'content-type': 'application/json' // 默认值
      },
      success: function (res) {
        var parsed = _this.parseData(res.data);
        if (parsed.length > 0) {
          //处理数据
          for (var i = 0; i < parsed.length; i++) {
            if(_this.data.num!=""&&_this.data.num!=null&&parsed[i].RN==_this.data.num){
              parsed[i].showflag = _this.data.numflag;
            }else{
              parsed[i].showflag = false;
            }
            var arr = [];
            arr = parsed[i].TUDE.split("、");
            parsed[i].arr = arr;
            if (i == parsed.length - 1) {
              // 赋值
              _this.setData({
                list: parsed,
                loading: false, // 关闭等待框
                update_time: _this.getDateTimeToString(), //更新时间
                has_msg: false, // 包含消息
                msg: '' //消息内容
              })
            }
          }
        } else {
          _this.setData({
            list: [],
            loading: false, // 关闭等待框
            update_time: _this.getDateTimeToString(), //更新时间
            has_msg: true, // 包含消息
            msg: '没有符合条件的数据。' //消息内容
          })
        }
      },
      fail: function () {
        _this.setData({
          list: [],
          loading: false, // 关闭等待框
          has_msg: true, // 包含消息
          msg: '获取服务器数据失败，请尝试刷新页面。' //消息内容
        })
      },
      complete: function () {
        wx.hideNavigationBarLoading() //完成停止加载
        wx.stopPullDownRefresh() //停止下拉刷新
      }
    })
  },


  requestUISettingData: function () {
    const _this = this;
    // 拼接请求url
    const url = this.data.wxapp_settings.settings_url +
                '?'+'_rnd=' + Math.random();
    // 请求数据
    wx.request({
      url: url,
      data: {},
      method: "GET",
      header: {
        'content-type': 'application/json' // 默认值
      },
      success: function (res) {
        console.log(res.data.ui_config)
          _this.setData({
            loading:false,
            ui_config: res.data.ui_config,
            wxapp_settings : res.data.wxapp_settings,
          });
          wx.setNavigationBarTitle({ title: _this.data.ui_config.ui_title }) 
      }
    })
  },

  requestUserData: function (res) {
    const _this = this;
    console.log(res)
    // 拼接请求url
    const url = _this.data.wxapp_settings.user_info_url + '?' +
              '_rnd=' + Math.random()+
              '&appkey='+ _this.data.wxapp_settings.config_appkey + 
              '&appsecret='+ _this.data.wxapp_settings.config_appsecret;
              
    // 请求数据
    wx.request({
      url: url,
      data: {
        ed: res.detail.encryptedData,
        iv: res.detail.iv,
        sk: app.globalData.userOpenId.sessionkey
      },
      method: "POST",
      header: {
        'content-type': 'application/json' // 默认值
      },
      success: function (res) {
        console.log(res.data)
        if (res.data.code == 200) {}
      }
    })
  },

  requestSubscribe: function (res) {
    const _this = this;
    const openid = res.data.detail.openid
    // 拼接请求url
    const url = _this.data.wxapp_settings.user_sub_url  + '?' + 
              '_rnd=' + Math.random()+
              '&oid=' + openid 
              '&appkey='+ _this.data.wxapp_settings.config_appkey + 
              '&appsecret='+ _this.data.wxapp_settings.config_appsecret;
    // 请求数据
    wx.request({
      url: url,
      data: {},
      method: "GET",
      header: {
        'content-type': 'application/json' // 默认值
      },
      success: function (res) {
        console.log(res.data)
        if (res.data.code == 200) {
          var subscribe = res.data.detail
          app.globalData.userSub = subscribe
          var convert_date = new Date(subscribe.sub_expire * 1000)
          subscribe.sub_expire_date = _this.getDateToString(convert_date)
          // 设置到全局变量
          _this.setData({
            loading:false,
            valid_date: subscribe.sub_expire_date,
          });
          _this.index();
        }
      }
    })
  },

  getUserInfo: function (e) {
    //console.log(e)
    const _this = this;
    app.globalData.userInfo = e.detail.userInfo
    if (e.detail.errMsg == "getUserInfo:ok") {
      // 将用户数据送到服务器
      _this.requestUserData(e);
      // 设置用户数据
      this.setData({
        userInfo: e.detail.userInfo,
        hasUserInfo: true
      })
      _this.taskFunction();
    } else {
      this.setData({
        hasUserInfo: false,
      })
    }
  },

  onLoad: function () {
    const _this = this;
    _this.setData({
      date: _this.timeFormat(new Date().getFullYear()) + "-" + _this.timeFormat(new Date().getMonth() + 1) + "-" + _this.timeFormat(new Date().getDate())
    });
    // 如果没有订阅数据
    if (app.globalData.userSub) {
      this.setData({
        valid_date: app.globalData.userSub.sub_expire_date
      })
      _this.index();
    } else {
      app.requestUserOpenIdCallBack = function (res) {
        _this.requestSubscribe(res)
      }
    }

  },

  //初始化加载
  index: function (e) {
    var _this = this;
    //判断用户信息是否存在
    if (app.globalData.userInfo) {
      this.setData({
        userInfo: app.globalData.userInfo,
        hasUserInfo: true
      })
      _this.taskFunction();
    } else if (this.data.canIUse) {
      // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
      // 所以此处加入 callback 以防止这种情况
      app.userInfoReadyCallback = function (res) {
        _this.setData({
          userInfo: res.userInfo,
          hasUserInfo: true
        })
        _this.taskFunction();
      }
      _this.setData({
        list: [],
        loading: false,
        date: _this.timeFormat(new Date().getFullYear()) + "-" + _this.timeFormat(new Date().getMonth() + 1) + "-" + _this.timeFormat(new Date().getDate())
      })
    } else {
      // 在没有 open-type=getUserInfo 版本的兼容处理
      wx.getUserInfo({
        success: res => {
          app.globalData.userInfo = res.userInfo
          this.setData({
            loading:false,
            userInfo: res.userInfo,
            hasUserInfo: true
          })
          _this.taskFunction();
        }
      })
    }
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {},

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    // wx.showNavigationBarLoading();
    // this.requestData();

    this.requestUISettingData();
    // 修改标题
    wx.setNavigationBarTitle({ title: this.data.ui_config.ui_title }) 
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
    wx.showNavigationBarLoading();
    clearInterval(this.data.task);
    this.taskFunction();
    wx.stopPullDownRefresh();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  },

  //日期选择控件回调函数
  bindDateChange: function (e) {
    this.setData({
      date: e.detail.value
    })
    clearInterval(this.data.task);
    this.taskFunction();
  },

  //显示概念板块详情
  showbtn: function (e) {
    var that = this;
    var RN = e.currentTarget.dataset.no;
    var datalist = that.data.list;
    var oldflag = datalist[RN - 1].showflag;
    var numflag = false;
    that.setData({
      
    });
    //处理数据
    for (var i = 0; i < datalist.length; i++) {
      if (RN == datalist[i].RN && !oldflag) {
        datalist[i].showflag = true;
        numflag = true;
      } else {
        datalist[i].showflag = false;
      }
      if (i == datalist.length - 1) {
        // 赋值
        that.setData({
          list: datalist,
          num: RN,
          numflag: numflag
        })
      }
    }
  },

  //日期选择控件回调函数
  rightChange: function () {
    var that = this;
    var a = that.data.rightshowflag;
    that.setData({
      rightshowflag: !a
    })
  },

  //定时任务随页面显示启动
  taskFunction: function () {
    var that = this;
    var hasUserInfo = that.data.hasUserInfo;
    var canIUse = that.data.canIUse;
    //判断当前是否处于登录状态，未登录不显示数据
    if (!hasUserInfo && canIUse) {
      //显示二维码
      that.setData({
        list: [],
        loading: false,
        date: that.timeFormat(new Date().getFullYear()) + "-" + that.timeFormat(new Date().getMonth() + 1) + "-" + that.timeFormat(new Date().getDate())
      })
    } else {
      //判断选中日期是不是当天
      if (new Date(that.data.date).getTime() != Date.parse(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + ' 08:00:00')) {
        that.setData({
          list: [],
          loading: false,
        });
        that.requestData();
      } else {
        //登录状态下判断时间区间
        //当前时间
        var nowdate = new Date().getTime();
        //当天00:00
        var time0000 = Date.parse(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + ' 00:00:00');
        //当天9:00
        var time0900 = Date.parse(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + ' 09:00:00');
        //当天9:30
        var time0930 = Date.parse(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + ' 09:30:00');
        //当天10:00
        var time1000 = Date.parse(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + ' 10:00:00');

        //当前时间在0:00-9:00，显示前一天的数据
        if (nowdate >= time0000 && nowdate < time0900) {
          var lastDate = new Date(nowdate - 86400000);
          var lastDateStr = that.timeFormat(lastDate.getFullYear()) + "-" + that.timeFormat(lastDate.getMonth() + 1) + "-" + that.timeFormat(lastDate.getDate());
          that.setData({
            date: lastDateStr
          });
          that.requestData();
        } else if (nowdate >= time0900 && nowdate < time0930) {
          //当前时间在9:00-9:30，空白显示
          that.setData({
            list: [],
            loading: false,
            date: that.timeFormat(new Date().getFullYear()) + "-" + that.timeFormat(new Date().getMonth() + 1) + "-" + that.timeFormat(new Date().getDate())
          })
        } else if (nowdate >= time0930 && nowdate < time1000) {
          //当前时间在9:30-10:00，需要判断是否有效期范围内
          var valid_date = that.data.valid_date;
          if (valid_date != "" && valid_date != "") {
            if (new Date(valid_date).getTime() >= new Date().getTime()) {
              //在有效期，当前时间
              that.setData({
                date: that.timeFormat(new Date().getFullYear()) + "-" + that.timeFormat(new Date().getMonth() + 1) + "-" + that.timeFormat(new Date().getDate())
              });
              that.requestData();
            } else {
              //不在有效期，显示二维码
              that.setData({
                list: [],
                loading: false,
                valid_date_flag: true,
                date: that.timeFormat(new Date().getFullYear()) + "-" + that.timeFormat(new Date().getMonth() + 1) + "-" + that.timeFormat(new Date().getDate())
              })
            }
          }
        } else {
          //当前时间在10:00-24:00，显示当天的数据
          that.setData({
            loading: false,
            date: that.timeFormat(new Date().getFullYear()) + "-" + that.timeFormat(new Date().getMonth()+1) + "-" + that.timeFormat(new Date().getDate())
          });
          that.requestData();
        }
        that.data.task = setTimeout(that.taskFunction, 2000);
      }
    }
  },

  //日期格式化
  timeFormat(param) { //小于10的格式化函数
    return param < 10 ? '0' + param : param;
  },
})