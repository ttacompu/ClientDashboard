USE [SyncDataCentral]
GO

/****** Object:  StoredProcedure [dbo].[spGetClientsForClientGroup]    Script Date: 7/20/2016 11:07:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetClientsForClientGroup]( 
	@ClientGroupNumber varchar(5)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT ClientNumber AS 'Client Number'
	  , ClientName AS 'Client Name' 
	FROM Client
	WHERE ClientGroupNumber = @ClientGroupNumber

END

GO


