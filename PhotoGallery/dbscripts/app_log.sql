create table App_Log
(
[Log_ID] uniqueidentifier ROWGUIDCOL  NOT NULL,
[Date] datetime,
[Message] varchar(8000) not null,
[LogLevel] varchar(100) not null
)

ALTER TABLE [dbo].[App_Log] ADD  CONSTRAINT [DF_App_Log_Log_ID]  DEFAULT (newid()) FOR [Log_ID] 
GO

ALTER TABLE [dbo].[App_Log] ADD  CONSTRAINT [DF_App_Log_Date]  DEFAULT (getdate()) FOR [Date] 
GO

--drop table App_Log

--delete from App_Log