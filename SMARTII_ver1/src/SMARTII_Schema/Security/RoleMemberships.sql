EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'SMARTIISA';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'SMARTIIDBA';


GO
EXECUTE sp_addrolemember @rolename = N'db_ddladmin', @membername = N'SMARTIIDBA';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'SMARTIIDBA';

