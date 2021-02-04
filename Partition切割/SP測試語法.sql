USE [OPTrade_Temp]
GO
SET STATISTICS PROFILE ON 
     SET STATISTICS IO ON 
   SET STATISTICS TIME ON
DECLARE @StartDate DATETIME,
@EndDate DATETIME,
@skip INT,
@take INT
		set @StartDate = N'2021-01-01'
		set @EndDate = N'2021-12-02'
		
		set @skip = 0
		set @take = 4000000
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
	
	count(distinct mid) AS TotalPeople,
	sum(cast(PerExchangePoint * ExchangeQty as bigint)) AS TotalPoint,
	sum(ExchangeQty) AS TotalExchangePoints ,
	VoucherType AS VoucherType	,
		COUNT(*) OVER () AS TotalRecords
	FROM ReportTradeActivityDetail
	WHERE
		  TradeTime >= @StartDate AND TradeTime < @EndDate
		  AND PartitionCode >= '2101' AND PartitionCode <= '2112'
		  AND VoucherType = 1 
		  AND ActivityItemCode > '1000'

	Group By
	
	ActivityID,
	VoucherHeaderCode,
	VoucherHeaderItemCode,
	DisplayCode,
	VoucherType
	order by TotalPoint
  		OFFSET     @skip ROWS       -- skip 10 rows
		FETCH NEXT @take ROWS ONLY; -- take 10 rows

  SET STATISTICS PROFILE OFF 
       SET STATISTICS IO OFF 
     SET STATISTICS TIME OFF
END