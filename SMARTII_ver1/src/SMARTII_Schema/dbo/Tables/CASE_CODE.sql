CREATE TABLE [dbo].[CASE_CODE] (
    [DATE]        CHAR (6) NOT NULL,
    [BU_CODE]     CHAR (3) NOT NULL,
    [SERIAL_CODE] INT      NOT NULL,
    CONSTRAINT [PK_CASE_CODE] PRIMARY KEY CLUSTERED ([DATE] ASC, [BU_CODE] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別案件滾號檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'滾號日 (yyMMdd)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CODE', @level2type = N'COLUMN', @level2name = N'DATE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CODE', @level2type = N'COLUMN', @level2name = N'BU_CODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件序號(5碼)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CODE', @level2type = N'COLUMN', @level2name = N'SERIAL_CODE';

