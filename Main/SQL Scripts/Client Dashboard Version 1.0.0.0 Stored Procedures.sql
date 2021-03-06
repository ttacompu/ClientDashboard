USE [ClientDashboard]
GO

/*

Add Enterprise Library Permissions


*/

GRANT EXECUTE ON [dbo].[AddCategory] TO [ClientDashboard_usr] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[ClearLogs] TO [ClientDashboard_usr] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[InsertCategoryLog] TO [ClientDashboard_usr] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[WriteLog] TO [ClientDashboard_usr] AS [dbo]
GO



IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spGetClientEntities') DROP PROCEDURE spGetClientEntities
GO

--=============================================
-- Author:		Alan J. Cohen
-- Create date: 8/12/2016
-- Description:	This store procedure will return the list of Client Names for the give Client Group Number. 
--				This will include all clients which have at least one active matter 
--				(Active or Closed matters for the selected client in the last 12 months).
--
-- Author:		Alexander C. Webbe
-- Create date: August 26, 2016
-- Description:	Tweaked Formatting
-- =============================================
CREATE PROCEDURE [dbo].[spGetClientEntities](
  @ClGrpNum varchar(10)
)
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

  SELECT DISTINCT Client.ClientName
	, Client.ClientNumber
  FROM SyncDataCentral.dbo.Matter
	INNER JOIN SyncDataCentral.dbo.Client on Client.ClientNumber = Matter.ClientNumber
  WHERE isnull(ClosedDate, '12/31/9999') > dateadd(month,-12,Getdate())
	and Client.ClientGroupNumber = @ClGrpNum
  Order by
	Client.ClientName

END
GO

GRANT EXECUTE ON [dbo].[spGetClientEntities] TO [ClientDashboard_usr] AS [dbo]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spClientMatterSearch') DROP PROCEDURE spClientMatterSearch
GO

-- =============================================
-- Author:		Alan J. Cohen
-- Create date: 5/16/2014
-- Description:	Get All Client Matters, Get All Client Matters by Client, Get All Client 
--		Matters by Client Group Number
--
-- Author:		Alexander C. Webbe
-- Create date: August 12, 2016
-- Description:	Refactor stored procedure to use SyncDataCentral instead of ClearyMart
--
-- Author:		Alexander C. Webbe
-- Create date: August 26, 2016
-- Description:	Return three resultsets to give client groups, clients and matters
-- =============================================
CREATE PROCEDURE [dbo].[spClientMatterSearch](
	@SearchString varchar(60) 
)
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

  SELECT ClientGroup.ClientGroupNumber
   , ClientGroup.ClientGroupName
   , Client.ClientNumber
   , Client.ClientName
   , Matter.ClientMatterNumber
   , Matter.MatterName
  FROM SyncDataCentral.dbo.ClientGroup
    INNER JOIN SyncDataCentral.dbo.Client ON ClientGroup.ClientGroupNumber = Client.ClientGroupNumber
	INNER JOIN SyncDataCentral.dbo.Matter ON Client.ClientNumber = Matter.ClientNumber
  WHERE Matter.IsPersonalCharge = 0 
    AND ClientGroup.ClientGroupNumber LIKE '%'+@SearchString+'%' 
	OR ClientGroup.ClientGroupName LIKE '%'+@SearchString+'%'
	OR Client.ClientNumber LIKE '%'+@SearchString+'%'
	OR Client.ClientName LIKE '%'+@SearchString+'%'
	OR Matter.ClientMatterNumber LIKE '%'+@SearchString+'%'
	OR Matter.MatterName LIKE '%'+@SearchString+'%'
  ORDER BY
	ClientGroup.ClientGroupNumber

END

GO
GRANT EXECUTE ON [dbo].[spClientMatterSearch] TO [ClientDashboard_usr] AS [dbo]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spApiKey_Select') DROP PROCEDURE spApiKey_Select
GO

-- =============================================
-- Author:		Thein Aung
-- Create date: August 8, 2016
-- =============================================  
CREATE PROCEDURE [dbo].[spApiKey_Select](
	 @ApiID bigint = NULL
)
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON; 

  SELECT 
		 [ApiID]
		,[ApiKey]
		,[ApplicationName]
		,[ExpirationDate]
  FROM	 [ApiKey]
  WHERE ([ApiID] = @ApiID OR @ApiID IS NULL)  
 
END
GO

GRANT EXECUTE ON [dbo].[spApiKey_Select] TO [ClientDashboard_usr] AS [dbo]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spApiKey_Delete') DROP PROCEDURE spApiKey_Delete
GO

-- =============================================
-- Author:		Praveen Codada
-- Create date: March 7, 2012
-- Description:	Delete single Api Key
-- =============================================  
CREATE PROCEDURE [dbo].[spApiKey_Delete]
	@ApiID bigint
AS
BEGIN
		DECLARE @application_data_exception_explanation varchar(500)

		DELETE	ApiKey
		WHERE	ApiID = @ApiID		
		
END
GO

GRANT EXECUTE ON [dbo].[spApiKey_Delete] TO [ClientDashboard_usr] AS [dbo]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'spApiKey_Update') DROP PROCEDURE spApiKey_Update
GO

-- =============================================
-- Author:		Thein Aung
-- Create date: August 18, 2016
-- Description:	Insert/Update single Api Key record
--
-- =============================================  
CREATE PROCEDURE [dbo].[spApiKey_Update]
	@ApiID bigint output,	
	@ApplicationName varchar(50)=null,
	@ExpirationDate datetime
AS
BEGIN
	
	IF not exists (select ApiID from ApiKey WHERE ApiID = @ApiID)
		BEGIN
		
			INSERT INTO 
			ApiKey(
			ApiKey,
			ApplicationName,
			ExpirationDate
			)
			VALUES (
			NEWID(),
			@ApplicationName,
			@ExpirationDate
			)
			-- Get newly inserted identity value ...
			SELECT  @ApiID = SCOPE_IDENTITY()
		END
		
	ELSE
		BEGIN
		
		if exists(select ApiID from ApiKey WHERE ApiID = @ApiID)
		Begin
			
			select @ApiID=ApiID from ApiKey WHERE ApiID = @ApiID

			UPDATE
				ApiKey
			SET
				ApplicationName = @ApplicationName,
				ExpirationDate = @ExpirationDate
			WHERE
				ApiID = @ApiID
		END
		ELSE
		  BEGIN
			select @ApiID=ApiID from ApiKey WHERE ApiID = @ApiID		  
		  END
	END	
END
GO

GRANT EXECUTE ON [dbo].[spApiKey_Update] TO [ClientDashboard_usr] AS [dbo]
GO