CREATE TABLE [dbo].[PPCLIFE_RESUME] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [ARRIVE_TYPE]     TINYINT        NOT NULL,
    [CONTENT]         NVARCHAR (MAX) NULL,
    [TYPE]            TINYINT        NOT NULL,
    [ITEM_CODE]       NVARCHAR (256) NULL,
    [ITEM_NAME]       NVARCHAR (256) NULL,
    [BATCH_NO]        NVARCHAR (256) NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    [EML_FILE_PATH]   NVARCHAR (256) NULL,
    CONSTRAINT [PK_PPCLIFE_RESUME] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'共同規則(EX:同批號同產品...)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'ARRIVE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄信內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0:通知 1:不通知', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品-國際條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'ITEM_CODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品-名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'ITEM_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品-批號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'BATCH_NO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄信檔案備份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_RESUME', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';

