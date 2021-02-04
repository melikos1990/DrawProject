USE [OPTrade]
GO
/****** Object:  StoredProcedure [dbo].[TradeTOForReport]    Script Date: 2021/2/3 上午 11:01:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		簡大鈞
-- Create date: 2020/02/11
-- Description:	轉Trade到Report報表
-- 1.因此寫法有問題-外部序號會被排除掉，故註解掉WHERE 的條件 (2020/10/21)
-- =============================================
ALTER PROCEDURE [dbo].[TradeTOForReport]
	@StartDate Datetime,
	@EndDate Datetime
AS
BEGIN

	SET NOCOUNT ON;

		insert into OPTrade..ReportTradeActivityDetail 
		SELECT
		TradeTime,
			t.ActivityItemCode,
		voucherHeader.Name,
		ct.Name,
		ca.Name,
		voucherHeader.ActivityID,
		voucherHeader.Code,
		i.Name,
		i.Code,
		i.DisplayCode,
		b.Name,
		voucherHeader.VoucherType,
		t.PerExchangePoint,
		t.ExchangeQty,
		t.MID,
		t.TransactionID,
		CONVERT(varchar(4),TradeTime,12),
		voucherHeader.ItemCode,
		ai.Name
		from OPTrade..Trade t
		left join OPMain..ActivityItem ai on t.ActivityItemCode = ai.Code --or t.ActivityItemCode = ai.ParentCode
		left join OPMain..OPStoreActivityItem oai on ai.Code = oai.Code
		left join OPMain..OPStoreItem i on i.Code = oai.ItemCode
		left join OPMain..ActivityItemVoucher av on av.ActivityItemCode = t.ActivityItemCode and (av.ExchangeType = t.ExchangeType OR av.ExchangeType = 4)
		left join OPMain..VoucherHeader voucherHeader on voucherHeader.code = av.VoucherHeaderCode
		left join OPMain..Brand b on b.ID = ai.BrandID
		left join OPMain..Category ca on ai.CategoryID = ca.ID
		left join OPMain..Category ct on ct.ID = ca.ParentID
		WHERE
		  (TradeTime >= @StartDate AND TradeTime < @EndDate)
		  AND TradeResultType = 1 --AND voucherHeader.ActivityID Is Not null
END