create table Error_Log
(
[Log_ID] uniqueidentifier ROWGUIDCOL  NOT NULL,
[Date] datetime,
[Message] varchar(8000) not null,
[Stacktrace] [varchar](8000) NULL, 
)

ALTER TABLE [dbo].[Error_Log] ADD  CONSTRAINT [DF_Error_Log_Log_ID]  DEFAULT (newid()) FOR [Log_ID] 
GO

ALTER TABLE [dbo].[Error_Log] ADD  CONSTRAINT [DF_Error_Log_Date]  DEFAULT (getdate()) FOR [Date] 
GO

--drop table Error_Log

--delete from Error_Log