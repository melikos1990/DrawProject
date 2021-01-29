CREATE TABLE [dbo].[CASE_SOURCE_CODE] (
    [DATE]        CHAR (6) NOT NULL,
    [SERIAL_CODE] INT      NOT NULL,
    CONSTRAINT [PK_CASE_SOURCE_CODE] PRIMARY KEY CLUSTERED ([DATE] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件來源滾號檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_CODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'滾號日 (yyMMdd)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_CODE', @level2type = N'COLUMN', @level2name = N'DATE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件序號(5碼)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_CODE', @level2type = N'COLUMN', @level2name = N'SERIAL_CODE';

