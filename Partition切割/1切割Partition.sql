DECLARE @PartitionMainName NVARCHAR(30)
DECLARE @FilePath NVARCHAR(MAX)
DECLARE @DBName NVARCHAR(30)
DECLARE @STR NVARCHAR(MAX)
DECLARE @SchemeSTR  NVARCHAR(MAX)
DECLARE @FunctionSTR  NVARCHAR(MAX)
DECLARE @TableName nvarCHAR(50)
DECLARE @IsFirstRow BIT
DECLARE @DelStr NVARCHAR(MAX)

DECLARE @TameM TABLE
(
	TName NVARCHAR(50)
)

INSERT INTO @TameM VALUES('01')
INSERT INTO @TameM VALUES('02')
INSERT INTO @TameM VALUES('03')
INSERT INTO @TameM VALUES('04')
INSERT INTO @TameM VALUES('05')
INSERT INTO @TameM VALUES('06')
INSERT INTO @TameM VALUES('07')
INSERT INTO @TameM VALUES('08')
INSERT INTO @TameM VALUES('09')
INSERT INTO @TameM VALUES('10')
INSERT INTO @TameM VALUES('11')
INSERT INTO @TameM VALUES('12')



DECLARE @TameYear TABLE
(
	TName NVARCHAR(50)
)
INSERT INTO @TameYear VALUES('00')
INSERT INTO @TameYear VALUES('01')
INSERT INTO @TameYear VALUES('02')
INSERT INTO @TameYear VALUES('03')
INSERT INTO @TameYear VALUES('04')
INSERT INTO @TameYear VALUES('05')
INSERT INTO @TameYear VALUES('06')
INSERT INTO @TameYear VALUES('07')
INSERT INTO @TameYear VALUES('08')
INSERT INTO @TameYear VALUES('09')
INSERT INTO @TameYear VALUES('10')
INSERT INTO @TameYear VALUES('11')
INSERT INTO @TameYear VALUES('12')
INSERT INTO @TameYear VALUES('13')
INSERT INTO @TameYear VALUES('14')
INSERT INTO @TameYear VALUES('15')
INSERT INTO @TameYear VALUES('16')
INSERT INTO @TameYear VALUES('17')
INSERT INTO @TameYear VALUES('18')
INSERT INTO @TameYear VALUES('19')
INSERT INTO @TameYear VALUES('20')
INSERT INTO @TameYear VALUES('21')
INSERT INTO @TameYear VALUES('22')
INSERT INTO @TameYear VALUES('23')
INSERT INTO @TameYear VALUES('24')
INSERT INTO @TameYear VALUES('25')
INSERT INTO @TameYear VALUES('26')
INSERT INTO @TameYear VALUES('27')
INSERT INTO @TameYear VALUES('28')
INSERT INTO @TameYear VALUES('29')
INSERT INTO @TameYear VALUES('30')
INSERT INTO @TameYear VALUES('31')
INSERT INTO @TameYear VALUES('32')
INSERT INTO @TameYear VALUES('33')
INSERT INTO @TameYear VALUES('34')
INSERT INTO @TameYear VALUES('35')
INSERT INTO @TameYear VALUES('36')
INSERT INTO @TameYear VALUES('37')
INSERT INTO @TameYear VALUES('38')
INSERT INTO @TameYear VALUES('39')
INSERT INTO @TameYear VALUES('40')
INSERT INTO @TameYear VALUES('41')
INSERT INTO @TameYear VALUES('42')
INSERT INTO @TameYear VALUES('43')
INSERT INTO @TameYear VALUES('44')
INSERT INTO @TameYear VALUES('45')
INSERT INTO @TameYear VALUES('46')
INSERT INTO @TameYear VALUES('47')
INSERT INTO @TameYear VALUES('48')
INSERT INTO @TameYear VALUES('49')
INSERT INTO @TameYear VALUES('50')
INSERT INTO @TameYear VALUES('51')
INSERT INTO @TameYear VALUES('52')
INSERT INTO @TameYear VALUES('53')
INSERT INTO @TameYear VALUES('54')
INSERT INTO @TameYear VALUES('55')
INSERT INTO @TameYear VALUES('56')
INSERT INTO @TameYear VALUES('57')
INSERT INTO @TameYear VALUES('58')
INSERT INTO @TameYear VALUES('59')
INSERT INTO @TameYear VALUES('60')
INSERT INTO @TameYear VALUES('61')
INSERT INTO @TameYear VALUES('62')
INSERT INTO @TameYear VALUES('63')
INSERT INTO @TameYear VALUES('64')
INSERT INTO @TameYear VALUES('65')
INSERT INTO @TameYear VALUES('66')
INSERT INTO @TameYear VALUES('67')
INSERT INTO @TameYear VALUES('68')
INSERT INTO @TameYear VALUES('69')
INSERT INTO @TameYear VALUES('70')
INSERT INTO @TameYear VALUES('71')
INSERT INTO @TameYear VALUES('72')
INSERT INTO @TameYear VALUES('73')
INSERT INTO @TameYear VALUES('74')
INSERT INTO @TameYear VALUES('75')
INSERT INTO @TameYear VALUES('76')
INSERT INTO @TameYear VALUES('77')
INSERT INTO @TameYear VALUES('78')
INSERT INTO @TameYear VALUES('79')
INSERT INTO @TameYear VALUES('80')
INSERT INTO @TameYear VALUES('81')
INSERT INTO @TameYear VALUES('82')
INSERT INTO @TameYear VALUES('83')
INSERT INTO @TameYear VALUES('84')
INSERT INTO @TameYear VALUES('85')
INSERT INTO @TameYear VALUES('86')
INSERT INTO @TameYear VALUES('87')
INSERT INTO @TameYear VALUES('88')
INSERT INTO @TameYear VALUES('89')
INSERT INTO @TameYear VALUES('90')
INSERT INTO @TameYear VALUES('91')
INSERT INTO @TameYear VALUES('92')
INSERT INTO @TameYear VALUES('93')
INSERT INTO @TameYear VALUES('94')
INSERT INTO @TameYear VALUES('95')
INSERT INTO @TameYear VALUES('96')
INSERT INTO @TameYear VALUES('97')
INSERT INTO @TameYear VALUES('98')
INSERT INTO @TameYear VALUES('99')

DECLARE @Tame2 TABLE
(
    TName2 nvarCHAR(50)
)

SET @FilePath='C:\Program Files\Microsoft SQL Server\MSSQL13.SD4LAB5\MSSQL\DATA\PartitionRTAD_'
SET @PartitionMainName='PartitionRTAD'
SET @DBName='OPTrade'


INSERT INTO @Tame2
SELECT a.TName + b.TName CC
FROM @TameYear a, @TameM b
ORDER BY CC ASC

--SELECT * FROM @Tame2 ORDER BY TName2 ASC

SET @FunctionSTR='CREATE PARTITION FUNCTION ['+@PartitionMainName +'Func](char(4)) AS RANGE LEFT FOR VALUES ('
SET @SchemeSTR= 'CREATE PARTITION SCHEME [' + @PartitionMainName +'Scheme] AS PARTITION ['+ @PartitionMainName +'Func] TO ('

SET @IsFirstRow=1


DECLARE dorlis_cursor CURSOR FOR 
    SELECT * FROM @Tame2 ORDER BY TName2 ASC
OPEN dorlis_cursor --開始run cursor                   
FETCH NEXT FROM dorlis_cursor INTO @TableName
WHILE @@FETCH_STATUS = 0 
BEGIN

	IF @IsFirstRow=0 
		SET @FunctionSTR +=' ,'
	ELSE
		SET @IsFirstRow=0
	SET @FunctionSTR += 'N''' + @TableName + ''''    
   
	 
     FETCH NEXT FROM dorlis_cursor INTO @TableName
END 
CLOSE dorlis_cursor
DEALLOCATE dorlis_cursor


SET @IsFirstRow=1
INSERT INTO @Tame2 VALUES('Other')

DECLARE dorlis_cursor CURSOR FOR 
    SELECT * FROM @Tame2 ORDER BY TName2 ASC
OPEN dorlis_cursor --開始run cursor                   
FETCH NEXT FROM dorlis_cursor INTO @TableName
WHILE @@FETCH_STATUS = 0 
BEGIN

	IF @IsFirstRow=0 
		SET @SchemeSTR +=' ,'
	ELSE
		SET @IsFirstRow=0

	SET @SchemeSTR += '[' + @PartitionMainName + 'FG' + @TableName +  ']'


	 -- Create Partition   MemberVoucher
	SET @STR = N' ALTER DATABASE '+ @DBName +' ADD FILEGROUP [' +@PartitionMainName + 'FG' + @TableName + ']; '
	SET @STR += N' ALTER DATABASE '+ @DBName +' '
	SET @STR += N' ADD FILE ( name=[' + @PartitionMainName + 'FG' + @TableName + '],filename=''' + @FilePath + @PartitionMainName + 'FG' + @TableName + '.ndf'',size=512KB) TO FileGroup[' + @PartitionMainName + 'FG' + @TableName + '] '
	
	--SET @DelStr = N'ALTER DATABASE [OPTrade] REMOVE FILEGROUP [' +@PartitionMainName + 'FG' + @TableName + ']'
	
	--select @STR
	Exec sp_executesql @STR
	--Exec sp_executesql @DelStr

     FETCH NEXT FROM dorlis_cursor INTO @TableName
END 
CLOSE dorlis_cursor
DEALLOCATE dorlis_cursor

SET @FunctionSTR+= ')'
--SELECT @FunctionSTR
Exec sp_executesql @FunctionSTR


SET @SchemeSTR+= ')'
--SELECT @SchemeSTR
Exec sp_executesql @SchemeSTR

