	SELECT TOP(200000) *
	FROM [OPTrade].[dbo].[ReportTradeActivityDetail_temp] WITH(NOLOCK)
	SELECT *
	FROM [OPTrade].[dbo].[ReportTradeActivityDetail]

	SELECT COUNT(1) FROM [OPTrade].[dbo].[ReportTradeActivityDetail_temp] WITH(NOLOCK)
	SELECT COUNT(1) FROM [OPTrade].[dbo].[ReportTradeActivityDetail]  WITH(NOLOCK)
    --TRUNCATE TABLE [OPTrade].[dbo].[ReportTradeActivityDetail_temp] 
	--DELETE FROM [OPTrade].[dbo].[ReportTradeActivityDetail] where PartitionCode <> '0001'


SELECT * 
FROM sys.partitions AS p
JOIN sys.tables AS t
ON  p.object_id = t.object_id
WHERE p.partition_id IS NOT NULL
AND t.name = 'ReportTradeActivityDetail_temp'

--pt_test = your partition table name
SELECT t.name AS TableName, i.name AS IndexName,r.value AS BoundaryValue , p.partition_number, p.partition_id, i.data_space_id, f.function_id, f.type_desc, r.boundary_id
FROM sys.tables AS t
JOIN sys.indexes AS i
    ON t.object_id = i.object_id
JOIN sys.partitions AS p
    ON i.object_id = p.object_id AND i.index_id = p.index_id 
JOIN  sys.partition_schemes AS s 
    ON i.data_space_id = s.data_space_id
JOIN sys.partition_functions AS f 
    ON s.function_id = f.function_id
LEFT JOIN sys.partition_range_values AS r 
    ON f.function_id = r.function_id and r.boundary_id = p.partition_number
WHERE t.name = 'ReportTradeActivityDetail_temp' AND i.type <= 1
ORDER BY p.partition_number;