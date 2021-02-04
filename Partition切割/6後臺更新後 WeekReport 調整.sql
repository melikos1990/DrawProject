USE [OPTrade]
GO
/****** Object:  StoredProcedure [dbo].[WeekReport]    Script Date: 2021/2/3 上午 11:21:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		林彥廷
-- Create date: 2019/05/14
-- Description:	兌換券日報表
-- =============================================
ALTER PROCEDURE [dbo].[WeekReport]
	-- Add the parameters for the stored procedure here
	@StartDate Datetime ,
	@EndDate Datetime   ,
	@PartitionCodeStart CHAR(4),
	@PartitionCodeEnd CHAR(4),
	@VoucherType INT   ,
	@ActivityItemCode VARCHAR(100)   ,
	@skip INT,
	@take INT
AS
BEGIN



	SET NOCOUNT ON;

	SELECT
	(CONVERT(VARCHAR(10),@StartDate, 111) + '-' + CONVERT(VARCHAR(10),@EndDate, 111)) AS [Date],
	MAX(ActivityName) AS ActivityName,
	MAX(LargeCategoryName) AS LargeCategory,
	MAX(SmallCategoryName) AS SmallCategory,
	ActivityID ,
	VoucherHeaderCode AS Code ,
	MAX(ActivityItemName) AS ItemName,
	VoucherHeaderItemCode AS ItemCode,
	DisplayCode AS DisplayCode,
	MAX(BrandName) AS BrandName,
	count(distinct mid) AS TotalPeople,
	sum(PerExchangePoint * ExchangeQty) AS TotalPoint,
	sum(ExchangeQty) AS TotalExchangePoints ,
	VoucherType AS VoucherType	,
		COUNT(*) OVER () AS TotalRecords
	FROM ReportTradeActivityDetail
	WHERE
		  TradeTime >= @StartDate AND TradeTime < @EndDate
		  AND PartitionCode >= @PartitionCodeStart AND PartitionCode <= @PartitionCodeEnd
		  AND (@VoucherType IS NULL OR VoucherType = @VoucherType)
		  AND (@ActivityItemCode IS NULL OR ActivityItemCode = @ActivityItemCode)
	Group By
	
	ActivityID,
	VoucherHeaderCode,
	VoucherHeaderItemCode,
	DisplayCode,
	VoucherType
	order by TotalPoint
  		OFFSET     @skip ROWS       -- skip 10 rows
		FETCH NEXT @take ROWS ONLY; -- take 10 rows

    -- Insert statements for procedure here
	--SELECT
	--(CONVERT(VARCHAR(10),@StartDate, 111) + '-' + CONVERT(VARCHAR(10),@EndDate, 111)) AS [Date],
	--vh.Name AS ActivityName,
	--ct.Name AS LargeCategory,
	--ca.Name AS SmallCategory,
	--vh.ActivityID AS ActivityID,
	--vh.Code AS Code,
	--ai.Name AS ItemName,
	--vh.ItemCode AS ItemCode,
	--i.DisplayCode AS DisplayCode,
	--b.Name AS BrandName,
	--count(distinct mid) AS TotalPeople,
	--sum(t.PerExchangePoint * t.ExchangeQty) AS TotalPoint,
	--sum(t.ExchangeQty) AS TotalExchangePoints,
	--vh.VoucherType, COUNT(*) OVER () AS TotalRecords
	--from OPTrade..Trade t
	--left join OPMain..ActivityItem ai on t.ActivityItemCode = ai.Code
	--left join OPMain..Item i on i.Code = ai.ItemCode
	--left join OPMain..ActivityItemVoucher av on av.ActivityItemCode = t.ActivityItemCode and av.ExchangeType = t.ExchangeType
	--left join OPMain..VoucherHeader vh on vh.code = av.VoucherHeaderCode
	--left join OPMain..Brand b on b.ID = ai.BrandID
	--left join OPMain..Category ca on ai.CategoryID = ca.ID
	--left join OPMain..Category ct on ct.ID = ca.ParentID
	--WHERE
	--  TradeTime BETWEEN @StartDate AND @EndDate
	--  and TradeResultType = 1 
	--Group By 
	--vh.Name,
	--vh.ActivityID,
	--vh.Code,
	--ai.Name,
	--vh.ItemCode,
	--i.DisplayCode,
	--b.Name, 
	--ca.Name, 
	--ct.Name,
	--vh.VoucherType
	--Order by TotalPoint
	--OFFSET     @skip ROWS       -- skip 10 rows
 --   FETCH NEXT @take ROWS ONLY; -- take 10 rows
END