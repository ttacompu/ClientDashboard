USE [SyncDataCentral]
GO
CREATE NONCLUSTERED INDEX IX_Matter_ClientNumber
ON [dbo].[Matter] ([ClientNumber])
INCLUDE ([ClientMatterNumber],[IsPersonalCharge],[MatterName])
GO
