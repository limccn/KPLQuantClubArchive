ALTER DATABASE stg01_db
ADD FILEGROUP fg_stg01_db CONTAINS MEMORY_OPTIMIZED_DATA;

ALTER DATABASE stg01_db
ADD FILE
(
   NAME = 'stg01_db_mem',
   FILENAME ='C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSVR\MSSQL\DATA\stg01_db.mem'
)
TO FILEGROUP [fg_stg01_db];
GO


CREATE TABLE [dbo].[TBL_STOCK_LATEST_ANALYSE] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [DATE]        NVARCHAR (10)      NULL,
    [TIME]        NVARCHAR (10)      NULL,
    [CODE]        NVARCHAR (20)      NULL,
    [NAME]        NVARCHAR (50)      NULL,
    [RATE]        MONEY NULL,
    [PRICE]       MONEY NULL,
    [CJE]         MONEY NULL,
    [RATIO]       MONEY NULL,
    [SPEED]       MONEY NULL,
    [SJLTP]       MONEY NULL,
    [TUDE]        NVARCHAR (500)     NULL,
    [BUY]         MONEY NULL,
    [SELL]        MONEY NULL,
    [ZLJE]        MONEY NULL,
    [QJZF]        MONEY NULL,
    [TAG]         NVARCHAR (100)     NULL,
    [JCF]         MONEY NULL,
    [JEF]         MONEY NULL,
    [JEZH]        MONEY NULL,
    [JEZH2]       MONEY NULL,
    [TL]          MONEY NULL,
    [QD]          MONEY NULL,
    
    [CREATE_TIME] SMALLDATETIME   DEFAULT (getdate()) NULL,
    PRIMARY KEY nonclustered ([ID] ASC)
)
WITH (  
        MEMORY_OPTIMIZED = ON,
        DURABILITY = SCHEMA_AND_DATA
);