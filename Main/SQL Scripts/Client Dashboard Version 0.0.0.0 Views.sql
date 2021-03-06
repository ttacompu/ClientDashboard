USE ClearyMart
GO

ALTER VIEW vwPartnersAndSeniorCounselBilledByClientGroup04041
AS

  select distinct Person.FullName AS 'Name'
    , Person.Title AS 'title'
    , Person.EmpID
	, Person.OfficeName AS 'Location'
  FROM dw_10YearTimeHistory
    INNER JOIN SyncDataCentral.dbo.Matter ON dw_10YearTimeHistory.tmatter = Matter.ClientMatterNumber
	INNER JOIN SyncDataCentral.dbo.Client ON Matter.ClientNumber = Client.ClientNumber
	INNER JOIN SyncDataCentral.dbo.Person ON dw_10YearTimeHistory.ttk = Person.EmpID
  WHERE Client.ClientGroupNumber = '04041'
    AND PositionSub IN ('Partner', 'Senior Counsel')

GO

ALTER VIEW vwPartnersAndSeniorCounselBilledByClientGroup
AS

  select distinct Person.FullName AS 'Name'
    , Person.Title AS 'title'
    , Person.EmpID
	, Person.OfficeName AS 'Location'
  FROM dw_10YearTimeHistory
    INNER JOIN SyncDataCentral.dbo.Matter ON dw_10YearTimeHistory.tmatter = Matter.ClientMatterNumber
	INNER JOIN SyncDataCentral.dbo.Client ON Matter.ClientNumber = Client.ClientNumber
	INNER JOIN SyncDataCentral.dbo.Person ON dw_10YearTimeHistory.ttk = Person.EmpID
  WHERE Client.ClientGroupNumber = '04021'
    AND PositionSub IN ('Partner', 'Senior Counsel')

GO