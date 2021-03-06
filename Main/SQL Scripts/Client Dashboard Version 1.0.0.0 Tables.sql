USE [ClientDashboard]
GO

/****** Object:  Table [dbo].[ApiKey]    Script Date: 8/12/2016 3:11:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ApiKey](
	[ApiID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ApiKey] [varchar](50) NULL,
	[ApplicationName] [varchar](50) NULL,
	[ExpirationDate] [datetime] NULL,
 CONSTRAINT [PK_ApiKey] PRIMARY KEY CLUSTERED 
(
	[ApiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--Seed Table
INSERT INTO [dbo].[ApiKey]
           ([ApiKey]
           ,[ApplicationName]
           ,[ExpirationDate])
     VALUES
           (NEWID()
           ,'SharePoint 2013 Client Dashboard'
           ,GetDate()+365)
GO



