﻿CREATE TABLE [dbo].[TBL_SIMULATE_ORDER]
(
	[ID]          INT            IDENTITY (1, 1) NOT NULL,
	[DATE]        NVARCHAR (10)  NULL,
    [TIME]        NVARCHAR (10)  NULL,
	[CHANNEL]     NVARCHAR (20)  DEFAULT ('JOINTQUANT') NOT NULL,
	[TYPE]        NVARCHAR (20)  DEFAULT ('BACKTEST') NOT NULL,
	[NONCE]       INT            DEFAULT (1) NOT NULL,
	[CODE]        NVARCHAR (20)  NULL,
    [NAME]        NVARCHAR (50)  NULL,
	[SIDE]        NVARCHAR (20)  NULL,
	[ACTION]      NVARCHAR (20)  DEFAULT ('OPEN') NOT NULL,
	[TRADE_TYPE]  NVARCHAR (10)  DEFAULT ('M') NOT NULL,
    [PRICE]       MONEY          NULL,
	[NUMBER]      MONEY          NULL,
	[AMOUNT]      MONEY          NULL,
	[FEE]         MONEY          NULL,
	[BALANCE]     MONEY          NULL,
	[CREATE_TIME] SMALLDATETIME  DEFAULT (getdate()) NULL,
    [UPDATE_TIME] SMALLDATETIME  DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
)
