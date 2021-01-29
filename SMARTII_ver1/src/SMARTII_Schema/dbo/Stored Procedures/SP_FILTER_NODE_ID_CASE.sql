-- =============================================
-- Author:		<洪有謙>
-- Create date: <2020/06/16>
-- Description:	<過濾出該節點的案件>
-- =============================================
CREATE PROCEDURE [dbo].[SP_FILTER_NODE_ID_CASE]
	@CaseList NVARCHAR(MAX),
	@NodeKeyList NVARCHAR(MAX)
AS
BEGIN

DECLARE @Statement NVARCHAR(MAX)
DECLARE @Where NVARCHAR(MAX)
DECLARE @Result TABLE (
	CaseID char(14)
)

	SET @Statement ='
		SELECT 
			C.[CASE_ID] 
		FROM 
			[dbo].[CASE] C 
		LEFT JOIN 
			[dbo].[CASE_COMPLAINED_USER] CCU 
			ON C.[CASE_ID] = CCU.[CASE_ID]
	'
	SET @Where =' WHERE 1 = 1 '

	--統藥部門
	IF (@NodeKeyList IS NOT NULL)
	BEGIN
	SET @Where = @Where + ' AND CCU.[NODE_ID] IN ('''+@NodeKeyList+''') '
	END

	--案件清單
	IF (@CaseList IS NOT NULL)
	BEGIN
	SET @Where = @Where + ' AND C.[CASE_ID] IN ('''+@CaseList+''') '
	END

	--過濾案件
	SET @Where = @Where + ' GROUP BY C.[CASE_ID] '

	SET @Statement = @Statement + @Where
	PRINT 'STATEMENT: ' + @Statement
	INSERT INTO @Result
	EXECUTE sp_executesql @Statement
    SELECT *
    FROM @Result
END
