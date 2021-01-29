CREATE TABLE [dbo].[CASE_PPCLIFE] (
    [CASE_ID]                CHAR (14)     NOT NULL,
    [ITEM_ID]                INT           NOT NULL,
    [IS_IGNORE]              BIT           NOT NULL,
    [ALLSAME_FINISH]         BIT           NULL,
    [DIFF_BATCHNO_FINISH]    BIT           NULL,
    [NOTHINE_BATCHNO_FINISH] BIT           NULL,
    [CREATE_DATETIME]        DATETIME      NOT NULL,
    [CREATE_USERNAME]        NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_CASE_PPCLIFE] PRIMARY KEY CLUSTERED ([CASE_ID] ASC, [ITEM_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'ITEM_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否無視', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'IS_IGNORE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'同批號同商品 已處理(EX: 1:已處理 0:未處理)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'ALLSAME_FINISH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不同批號同商品 已處理(EX: 1:已處理 0:未處理)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'DIFF_BATCHNO_FINISH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'無批號同商品 已處理(EX: 1:已處理 0:未處理)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_PPCLIFE', @level2type = N'COLUMN', @level2name = N'NOTHINE_BATCHNO_FINISH';

