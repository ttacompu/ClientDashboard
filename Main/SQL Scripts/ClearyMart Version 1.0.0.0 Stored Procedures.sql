USE ClearyMart

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'TitlesTableType' AND ss.name = N'dbo')
CREATE TYPE [dbo].[TitlesTableType] AS TABLE(
	[Name] [varchar](50) NULL
)
GO

GRANT CONTROL ON TYPE::[dbo].[TitlesTableType] TO [ClientDashboard_usr] AS [dbo]
GO

GRANT REFERENCES ON TYPE::[dbo].[TitlesTableType] TO [ClientDashboard_usr] AS [dbo]
GO


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spGetTimekeepersBilled') DROP PROCEDURE spGetTimekeepersBilled
GO

--=============================================
-- Author:		Alexander C. Webbe
-- Create date: August 29, 2016
-- Description:	Get timekeepers who have billed time
--	Pass 'Partner', 'Senior Counsel' into table parameter
-- =============================================
CREATE PROCEDURE [dbo].[spGetTimekeepersBilled](
    @ClientGroupNumber char(6)
  , @StartDate date 
  , @EndDate date 
  , @Threshold decimal(16,2) = NULL
  , @TitlesTable TitlesTableType READONLY
)
AS
BEGIN

  DECLARE @Rows int
  SET @Rows = (SELECT COUNT(Name) FROM @TitlesTable)

  IF( @Rows > 0 )
	BEGIN    
	  SELECT DISTINCT Person.FullName AS 'Name'
		, Person.Title AS 'title'
		, Person.EmpID
		, Person.OfficeName AS 'Location'
	  FROM dw_10YearTimeHistory
		INNER JOIN SyncDataCentral.dbo.Matter ON dw_10YearTimeHistory.tmatter = Matter.ClientMatterNumber
		INNER JOIN SyncDataCentral.dbo.Client ON Matter.ClientNumber = Client.ClientNumber
		INNER JOIN SyncDataCentral.dbo.Person ON dw_10YearTimeHistory.ttk = Person.EmpID
	  WHERE Client.ClientGroupNumber = @ClientGroupNumber
		AND Person.PositionSub IN (SELECT Name FROM @TitlesTable)
		AND (dw_10YearTimeHistory.tworkhrs >= @Threshold OR @Threshold IS NULL)
	END
  ELSE
    BEGIN
	  SELECT DISTINCT Person.FullName AS 'Name'
		, Person.Title AS 'title'
		, Person.EmpID
		, Person.OfficeName AS 'Location'
	  FROM dw_10YearTimeHistory
		INNER JOIN SyncDataCentral.dbo.Matter ON dw_10YearTimeHistory.tmatter = Matter.ClientMatterNumber
		INNER JOIN SyncDataCentral.dbo.Client ON Matter.ClientNumber = Client.ClientNumber
		INNER JOIN SyncDataCentral.dbo.Person ON dw_10YearTimeHistory.ttk = Person.EmpID
	  WHERE Client.ClientGroupNumber = @ClientGroupNumber
	    AND (dw_10YearTimeHistory.tworkhrs >= @Threshold OR @Threshold IS NULL)
	END
END
GO

GRANT EXECUTE ON [dbo].[spGetTimekeepersBilled] TO [ClientDashboard_usr] AS [dbo]
GO
