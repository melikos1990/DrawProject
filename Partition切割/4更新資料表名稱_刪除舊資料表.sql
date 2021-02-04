USE OPTrade;   
GO  
--EXEC sp_rename 'dbo.ReportTradeActivityDetail', 'ReportTradeActivityDetail_Origin'; 
--EXEC sp_rename 'dbo.ReportTradeActivityDetail_New', 'ReportTradeActivityDetail'; 
--DROP TABLE dbo.ReportTradeActivityDetail_Origin --確認沒問題後即可刪除