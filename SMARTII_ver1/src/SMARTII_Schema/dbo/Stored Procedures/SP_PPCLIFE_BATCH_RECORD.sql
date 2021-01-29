
-- =============================================
-- Author:		Cary 
-- Create date: 2020/01/31
-- Description:	統一藥品-紀錄，二次查詢，再查詢出反應品牌、姓名、電話、轉派人員、回覆內容
--需要二次查詢是因為資料有經過編碼後儲存
-- =============================================
CREATE PROCEDURE [dbo].[SP_PPCLIFE_BATCH_RECORD]
	-- Add the parameters for the stored procedure here
	--時間區間
	@StarTime DateTime,
	@EndTime DateTime,
	--一般客訴、重大客訴
	@ClassificationID nvarchar(1024),
	--來源狀態
	@SourceType tinyint,
	-- BU ID
	@NodeID tinyint
AS
DECLARE @statement nvarchar(MAX)
DECLARE @join nvarchar(MAX)
DECLARE @where nvarchar(MAX)
DECLARE @Results TABLE (
CaseID char(14),
CreatDateTime datetime,
ClassificationID int,
CaseContent nvarchar(MAX),
FinishContent nvarchar(MAX),
ApplyUserName nvarchar(20),
FinishDateTime datetime,
SourceType tinyint
)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @statement ='
	SELECT 
	C.CASE_ID AS CaseID,
	C.CREATE_DATETIME AS CreatDateTime,
	C.QUESION_CLASSIFICATION_ID AS ClassificationID,
	C.CONTENT AS CaseContent,
	C.FINISH_CONTENT AS FinishContent,
	C.APPLY_USERNAME AS ApplyUserName,
	C.FINISH_DATETIME AS FinishDateTime,
	CS.SOURCE_TYPE
	from [dbo].[CASE] C 
	Inner Join CASE_SOURCE CS on C.SOURCE_ID =CS.SOURCE_ID
	'
	SET @where =' where 1 = 1 and C.IS_REPORT = 1 '

	--時間區間
	SET @where = @where+' and C.NODE_ID = ' + CONVERT(varchar, @NodeID)

	--時間區間
	IF (@StarTime IS NOT NULL and @EndTime IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.CREATE_DATETIME >='''+ CONVERT(varchar, @StarTime)+''' and C.CREATE_DATETIME <='''+ CONVERT(varchar, @EndTime)+''''
	END
	--一般客訴、重大客訴
	IF (@ClassificationID IS NOT NULL)
	BEGIN
	set @where = @where + ' and C.QUESION_CLASSIFICATION_ID NOT in ('+@ClassificationID+')'
	END
	--來源型態
	IF (@SourceType IS NOT NULL)
	BEGIN
	SET @where = @where+' and CS.SOURCE_TYPE =' + @SourceType
	END
	SET @statement = @statement + @where
	PRINT 'STATEMENT: ' + @statement
	INSERT INTO @Results
	EXECUTE sp_executesql @statement
	 --Finally, select the results of the table variable
    SELECT *
    FROM @Results
END

