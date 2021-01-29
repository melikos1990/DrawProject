CREATE TABLE [dbo].[PPCLIFE_EFFECTIVE_SUMMARY] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [ARRIVE_TYPE]     TINYINT        NOT NULL,
    [ITEM_ID]         INT            NOT NULL,
    [BATCH_NO]        NVARCHAR (255) NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_PPCLIFE_EFFECTIVE_SUMMARY_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'共同規則(EX:同批號同產品...)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'ARRIVE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'ITEM_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'批號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPCLIFE_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'BATCH_NO';

