



-- =============================================
-- Author:		Cary
-- Create date: 2020/01/20
-- Description:	轉派案件查詢(總部、門市、廠商)歷程:單位溝通，依照以下條件先篩選一次
-- 連絡者、被反應者，一對多的資料在C# 二查後過濾
-- =============================================
CREATE PROCEDURE [dbo].[SP_ASSIGNMENT_COMPLAINT_COMMUNICATE_HEADQUARTER_STORE]
	-- Add the parameters for the stored procedure here
	--企業別
	@NodeID int,
	--歷程狀態
	@Type int,
	--案件編號
	@CaseID varchar (50),
	--立案時間
	@CreateStarTime DateTime,
	@CreateEndTime DateTime,
	--通知時間
	@NoticeDateStarTime DateTime,
	@NoticeDateEndTime  DateTime,
	--通知內容
	@NoticeContent nvarchar(1024),
	--案件內容
	@CaseContent nvarchar(1024),
	--分類
	@CaseClassificationIDGroup nvarchar(1024),
	--組織節點
	@HeadNodeID nvarchar(1024),
	--總部、門市、廠商
	@OrganizationType tinyint
AS
DECLARE @statement nvarchar(MAX)
DECLARE @join nvarchar(MAX)
DECLARE @where nvarchar(MAX)
DECLARE @Results TABLE (
NodeKey char(3),
NodeName nvarchar(20),
CaseID char(14),
CaseType tinyint,
CaseWarningName nvarchar(20),
IncomingDateTime datetime,
IsPrevention bit,
IsAttension bit,
CaseSourceType tinyint,
CreateDateTime datetime,
CaseContent nvarchar(MAX),
ExpectDateTime datetime,
PromiseDateTime datetime,
NoticeDateTime datetime,
NoticeContent nvarchar(2048),
ApplyUserName nvarchar(20),
ClassificationID int,
IdentityID int
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
	C.CASE_ID AS CaseID,
	C.CASE_TYPE AS CaseType,
	CW.Name AS CaseWarningName,
	CS.INCOMING_DATETIME AS IncomingDateTime,
	CS.IS_PREVENTION  AS IsPrevention,
	C.IS_ATTENSION AS IsAttension,
	CS.SOURCE_TYPE AS CaseSourceType,
	C.CREATE_DATETIME AS CreateDateTime,
	C.CONTENT AS CaseContent,
	C.EXPECT_DATETIME AS ExpectDateTime,
	C.PROMISE_DATETIME AS PromiseDateTime,
	CACN.NOTICE_DATETIME AS NoticeDateTime,
	CACN.CONTENT AS NoticeContent,
	C.APPLY_USERNAME AS ApplyUserName,
	C.QUESION_CLASSIFICATION_ID AS ClassificationID,
	CACN.ID AS IdentityID

	from [dbo].[CASE] C 
	Left Join CASE_SOURCE CS on C.SOURCE_ID =CS.SOURCE_ID
	Left Join CASE_WARNING CW on C.CASE_WARNING_ID =CW.ID
	Inner Join CASE_ASSIGNMENT_COMMUNICATE CACN on C.CASE_ID =CACN.CASE_ID
	'

	SET @where =' where 1 = 1 '
	--企業別 必填
	SET @join ='Inner Join (select H.NAME,H.NODE_ID,H.NODE_KEY from HEADQUARTERS_NODE H where H.NODE_ID = '+CONVERT(varchar, @NodeID)+' ) H on C.NODE_ID =H.NODE_ID '
	--案件編號
	IF (@CaseID IS NOT NULL and @CaseID != '')
	BEGIN
	SET @where = @where+' and C.CASE_ID like ''%'+ @CaseID +'%'''
	END
	--立案時間
	IF (@CreateStarTime IS NOT NULL and @CreateEndTime IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.CREATE_DATETIME >='''+ CONVERT(varchar, @CreateStarTime)++''' and C.CREATE_DATETIME <='''+ CONVERT(varchar, @CreateEndTime)+''''
	END
	-- 通知時間 
	IF (@NoticeDateStarTime IS NOT NULL and @NoticeDateEndTime IS NOT NULL)
	BEGIN
	SET @where = @where+' and CACN.NOTICE_DATETIME >='''+ CONVERT(varchar, @NoticeDateStarTime)++''' and CACN.NOTICE_DATETIME <='''+ CONVERT(varchar, @NoticeDateEndTime)+''''
	END
	--通知內容
	IF(@NoticeContent IS NOT NULL and @NoticeContent != '')
	BEGIN
	SET @where = @where+' and CACN.CONTENT like ''%'+  @NoticeContent +'%'''
	END
	--案件內容
	IF (@CaseContent IS NOT NULL and @CaseContent != '')
	BEGIN
	SET @where = @where+' and C.CONTENT like ''%'+ @CaseContent +'%'''
	END
	--分類
	IF (@CaseClassificationIDGroup IS NOT NULL)
	BEGIN
	SET @where = @where+' and C.QUESION_CLASSIFICATION_ID in ('+@CaseClassificationIDGroup+')'
	END
	--組織節點
	IF (@HeadNodeID IS NOT NULL and @OrganizationType IS NOT NULL)
	BEGIN
	SET @where = @where+'and (C.[CASE_ID] in (SELECT distinct c.[CASE_ID] FROM [dbo].[CASE] c 
	inner join [dbo].[CASE_COMPLAINED_USER] cc on c.CASE_ID = cc.CASE_ID and (cc.UNIT_TYPE =1 OR cc.UNIT_TYPE = 2) and cc.ORGANIZATION_TYPE ='+CONVERT(varchar,@OrganizationType)
	+' and cc.NODE_ID in('''+  @HeadNodeID +'''))
	or C.[CASE_ID] in (select distinct ac.[CASE_ID]  from [dbo].[CASE] ac inner  join [dbo].[CASE_ASSIGNMENT_COMPLAINT_NOTICE_USER] ca on ca.CASE_ID = ac.CASE_ID 
	and ca.NODE_ID in('''+  @HeadNodeID +''') and ca.ORGANIZATION_TYPE ='+CONVERT(varchar,@OrganizationType)+'))'
	END


	SET @statement = @statement +@join + @where
	PRINT 'STATEMENT: ' + @statement
	INSERT INTO @Results
	EXECUTE sp_executesql @statement
	 --Finally, select the results of the table variable
    SELECT *
    FROM @Results

END




