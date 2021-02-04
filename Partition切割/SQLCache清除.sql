DBCC DROPCLEANBUFFERS;

select sysObj.name,*
from sys.dm_os_buffer_descriptors bufferDescriptors
INNER JOIN sys.allocation_units AllocUnits ON bufferDescriptors.allocation_unit_id = AllocUnits.allocation_unit_id
INNER JOIN sys.partitions Partitions ON AllocUnits.container_id = Partitions.hobt_id
INNER JOIN sys.objects sysObj ON Partitions.object_id = sysObj.object_id
WHERE bufferDescriptors.database_id = DB_ID()
AND sysObj.is_ms_shipped = 0