﻿CREATE TABLE [dbo].[TBL_USER_INFO] (
    [ID]          INT              IDENTITY (1, 1) NOT NULL,
    [USER_ID]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [CHANNEL]     NVARCHAR (20)    DEFAULT ('WeiXin') NOT NULL,
    [APPID]       NVARCHAR (200) NULL,
    [OPENID]      NVARCHAR (200)   NULL,
    [UNIONID]     NVARCHAR (200)   NULL,
    [NAME]        NVARCHAR (50)    NULL,
    [NICK_NAME]   NVARCHAR (50)    NULL,
    [GENDER]      NVARCHAR (4)     NULL,
    [CITY]        NVARCHAR (50)    NULL,
    [PROVINCE]    NVARCHAR (50)    NULL,
    [COUNTRY]     NVARCHAR (50)    NULL,
    [MOBILE]      NVARCHAR (20)    NULL,
    [AVATAR_URL]  NVARCHAR (400)   NULL,
    [COUNT]       INT              DEFAULT ((0)) NULL,
    [CREATE_TIME] SMALLDATETIME    DEFAULT (getdate()) NULL,
    [UPDATE_TIME] SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO

CREATE TABLE [dbo].[TBL_USER_ACCESS_TOKEN]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [APPID]       NVARCHAR (200) NULL,
	[OPENID] NVARCHAR(200) NULL, 
    [UNIONID] NVARCHAR(200) NULL,
    [ACCESS_TOKEN] NVARCHAR(200) NULL,  
    [EXPIRE] INT NULL,
	[CREATE_TIME] SMALLDATETIME DEFAULT (getdate()) NULL,
)

GO

CREATE TABLE [dbo].[TBL_SUBSCRIBE] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [APPID]       NVARCHAR (200) NULL,
    [OPENID]      NVARCHAR (200) NULL,
    [UNIONID]     NVARCHAR (200) NULL,
    [SUB_TYPE]    NVARCHAR (10)  DEFAULT ('free') NULL,
    [SUB_EXPIRE]  INT            NULL,
    [CREATE_TIME] SMALLDATETIME  DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
)

