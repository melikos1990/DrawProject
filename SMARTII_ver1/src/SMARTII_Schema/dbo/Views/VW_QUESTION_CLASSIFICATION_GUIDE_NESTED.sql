﻿

CREATE VIEW [dbo].[VW_QUESTION_CLASSIFICATION_GUIDE_NESTED]   AS
WITH QUESTION_CLASSIFICATION_NESTED(
	NODE_ID,
	ID ,
	PARENT_ID ,
	[NAME],
	ORGANIZATION_TYPE,
	LEVEL,
	PARENT_PATH,
	PARENT_PATH_NAME)
AS(
  SELECT 
	NODE_ID ,
	ID , 
	PARENT_ID ,
	[NAME],
	ORGANIZATION_TYPE , 
	1 AS LEVEL,
	CONVERT(nvarchar(128), ID) AS PARENT_PATH,
	CONVERT(nvarchar(128), [NAME]) AS  PARENT_PATH_NAME  
  FROM [dbo].QUESTION_CLASSIFICATION
  WHERE PARENT_ID IS NULL
  UNION ALL
  SELECT 
    S.NODE_ID ,
	S.ID , 
	S.PARENT_ID ,
	S.[NAME],
	S.ORGANIZATION_TYPE , 
	P.LEVEL + 1 AS LEVEL ,
	CONVERT(nvarchar(128),  P.PARENT_PATH + '@' + CONVERT(nvarchar(128),S.ID)),
	CONVERT(nvarchar(128),   P.PARENT_PATH_NAME + '@' + S.NAME)
  FROM [dbo].QUESTION_CLASSIFICATION S
  JOIN QUESTION_CLASSIFICATION_NESTED P
  ON S.PARENT_ID = P.ID
)
SELECT 
    ISNULL(row_number() over(order by ID),0) as [INDEX],
	S.CLASSIFICATION_ID AS ID,
	S.CONTENT,
	S.NODE_ID AS NODE_ID,
	B.NAME AS NODE_NAME,
	S.CLASSIFICATION_ID AS CLASSIFICATION_ID,
	M.NAME AS  CLASSIFICATION_NAME,
	M.PARENT_PATH AS PARENT_PATH,
	M.PARENT_PATH_NAME AS PARENT_PATH_NAME,
	S.CREATE_DATETIME,
	S.CREATE_USERNAME,
	S.UPDATE_DATETIME,
	S.UPDATE_USERNAME

FROM [dbo].QUESTION_CLASSIFICATION_GUIDE S JOIN 
QUESTION_CLASSIFICATION_NESTED M 
ON S.CLASSIFICATION_ID = M.ID
JOIN (
SELECT [NODE_ID] , 
[ORGANIZATION_TYPE] ,  
[NAME] FROM [dbo].HEADQUARTERS_NODE) B
ON S.NODE_ID = B.NODE_ID
AND S.ORGANIZATION_TYPE = B.ORGANIZATION_TYPE

