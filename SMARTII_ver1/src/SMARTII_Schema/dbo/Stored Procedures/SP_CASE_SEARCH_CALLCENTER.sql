
-- =============================================
-- Author:		Cary
-- Create date: 2020/01/17
-- Description:	案件查詢(客服) 依照以下條件先篩選一次
-- 連絡者、被反應者、案件標籤、反映單號、結案處置，一對多的資料在C# 二查後過濾
-- =============================================
CREATE PROCEDURE [dbo].[SP_CASE_SEARCH_CALLCENTER]
	-- Add the condition parameters for the stored procedure here
	--企業別 必填
	@NodeID int,
	--案件來源
	@CaseSourceType int,
	--案件編號
	@CaseID char(14),
	--立案時間
	@CreateStarTime DateTime,
	@CreateEndTime DateTime,
	--案件內容
	@CaseContent nvarchar(1024),
	--結案內容
	@FinishContent nvarchar(2048),
	--負責人ID
	@ApplyUserID nvarchar(256),
	--期望時間
	@ExpectDateStarTime DateTime,
	@ExpectDateEndTime  DateTime,
	--案件等級
	@CaseWarningID int ,
	--案件狀態
	@CaseType int,
	--分類
	@CaseClassificationIDGroup nvarchar(1024),
	--組織節點
	@GroupID nvarchar(1024)
AS
DECLARE @statement nvarchar(MAX)
DECLARE @join nvarchar(MAX)
DECLARE @where nvarchar(MAX)
DECLARE @Concat nvarchar(MAX)
DECLARE @Results TABLE (
NodeKey char(3),
NodeName nvarchar(20),
SourceType tinyint,
CaseID char(14),
CreateDateTime datetime,
IncomingDateTime datetime,
ExpectDateTime datetime,
ApplyUserName nvarchar(20),
CaseType tinyint,
CaseWarningName nvarchar(20),
IsPrevention bit,
IsAttension bit,
CaseContent nvarchar(MAX),
PromiseDateTime datetime,
FinishContent nvarchar(MAX),
FinishDateTime datetime,
ClassificationID int
)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @statement ='
	SELECT 
	H.NODE_KEY AS NodeKey,
	H.NAME AS NodeName,
	CS.SOURCE_TYPE AS SourceType,
	C.CASE_ID AS CaseID,
	C.CREATE_DATETIME AS CreateDateTime,
	CS.INCOMING_DATETIME AS IncomingDateTime,
	C.EXPECT_DATETIME AS ExpectDateTime,
	C.APPLY_USERNAME AS ApplyUserName,
	C.CASE_TYPE AS CaseType,
	CW.Name AS CaseWarningName,
	CS.IS_PREVENTION  AS IsPrevention,
	C.IS_ATTENSION AS IsAttension,
	C.CONTENT AS CaseContent,
	C.PROMISE_DATETIME AS PromiseDateTime,
	C.FINISH_CONTENT AS FinishContent,
	C.FINISH_DATETIME AS FinishDateTime,
	C.QUESION_CLASSIFICATION_ID AS ClassificationID
	from [dbo].[CASE] C
	'
	SET @where =' where 1 = 1 '
	--企業別 必填
	SET @join ='Inner Join (select H.NAME,H.NODE_ID,H.NODE_KEY from HEADQUARTERS_NODE H where H.NODE_ID = '+CONVERT(varchar, @NodeID)+' ) H on C.NODE_ID =H.NODE_ID '
	--案件來源
	IF (@CaseSourceType IS NOT NULL)
	BEGIN
	SET @join =@join +'Inner Join (select CS.SOURCE_ID,CS.SOURCE_TYPE,CS.IS_PREVENTION,CS.INCOMING_DATETIME from CASE_SOURCE CS where CS.SOURCE_TYPE = '''+CONVERT(varchar, @CaseSourceType)+''' ) CS on C.SOURCE_ID =CS.SOURCE_ID '
	END
	ELSE
	BEGIN
	SET @join =@join +'Inner Join (select CS.SOURCE_ID,CS.SOURCE_TYPE,CS.IS_PREVENTION,CS.INCOMING_DATETIME from CASE_SOURCE CS ) CS on C.SOURCE_ID =CS.SOURCE_ID '
	END
	--案件編號
	IF (@CaseID IS NOT NULL and @CaseID !='')
	BEGIN
	SET @where = @where+' and C.CASE_ID like ''%'+ REPLACE(@CaseID,' ','') +'%'''
	END
	--立案時間
	IF (@CreateStarTime IS NOT NULL and @CreateEndTime IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.CREATE_DATETIME >='''+ CONVERT(varchar, @CreateStarTime)++''' and C.CREATE_DATETIME <='''+ CONVERT(varchar, @CreateEndTime)+''''
	END
	--案件內容
	IF (@CaseContent IS NOT NULL and @CaseContent !='')
	BEGIN
	SET @where = @where+' and C.CONTENT like ''%'+ @CaseContent +'%'''
	END
	--結案內容
	IF (@FinishContent IS NOT NULL and @FinishContent !='')
	BEGIN
	SET @where = @where+' and C.FINISH_CONTENT like ''%'+ @FinishContent +'%'''
	END
	--負責人
	IF (@ApplyUserID IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.APPLY_USER_ID  ='''+ @ApplyUserID +''''
	END
	-- 期望期限
	IF (@ExpectDateStarTime IS NOT NULL and @ExpectDateEndTime IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.EXPECT_DATETIME >='''+ CONVERT(varchar, @ExpectDateStarTime)++''' and C.EXPECT_DATETIME <='''+ CONVERT(varchar, @ExpectDateEndTime)+''''
	END
	--案件等級
	IF (@CaseWarningID IS NOT NULL)
	BEGIN
	SET @join =@join +'Inner Join (select CW.ID,CW.NAME from CASE_WARNING CW where CW.ID = '+ CONVERT(varchar, @CaseWarningID) +') CW on C.CASE_WARNING_ID =CW.ID '
	END
	ELSE
	BEGIN
	SET @join =@join +'Inner Join (select CW.ID,CW.NAME from CASE_WARNING CW ) CW on C.CASE_WARNING_ID =CW.ID '
	END
	--案件狀態
	IF (@CaseType IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.CASE_TYPE ='+ CONVERT(varchar, @CaseType)
	END
	--分類
	IF (@CaseClassificationIDGroup IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.QUESION_CLASSIFICATION_ID in ('+@CaseClassificationIDGroup+')'
	END
	--組織節點
	IF (@GroupID IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.GROUP_ID in('''+ @GroupID +''')'
	END

	SET @statement = @statement + @join + @where
	PRINT 'STATEMENT: ' + @statement
	INSERT INTO @Results
	EXECUTE sp_executesql @statement
	 --Finally, select the results of the table variable
    SELECT *
    FROM @Results
END

