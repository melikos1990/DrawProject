CREATE TABLE [dbo].[CASE_ITEM] (
    [CASE_ID]  CHAR (14)      NOT NULL,
    [ITEM_ID]  INT            NOT NULL,
    [JCONTENT] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CASE_ITEM] PRIMARY KEY CLUSTERED ([CASE_ID] ASC, [ITEM_ID] ASC),
    CONSTRAINT [FK_CASE_ITEM_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID]),
    CONSTRAINT [FK_CASE_ITEM_ITEM] FOREIGN KEY ([ITEM_ID]) REFERENCES [dbo].[ITEM] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件-其他資訊(商品相關)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ITEM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ITEM', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ITEM', @level2type = N'COLUMN', @level2name = N'ITEM_ID';

