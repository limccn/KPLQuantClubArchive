import os

#if os.environ['RUNNING_STATE'] == 'RealTrade':
#    from ths_shipan.ths_android import *
#elif os.environ['RUNNING_STATE'] == 'SimulateTrade':
#    from ths_moni.ths_android import *
#else:
#    from ths_moni.ths_android import *

from .ths_shipan.ths_android import *