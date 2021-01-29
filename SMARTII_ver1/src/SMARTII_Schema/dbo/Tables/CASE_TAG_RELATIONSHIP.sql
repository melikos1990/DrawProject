CREATE TABLE [dbo].[CASE_TAG_RELATIONSHIP] (
    [CASE_TAG_ID] INT       NOT NULL,
    [CASE_ID]     CHAR (14) NOT NULL,
    CONSTRAINT [PK_CASE_TAG_RELATIONSHIP] PRIMARY KEY CLUSTERED ([CASE_TAG_ID] ASC, [CASE_ID] ASC),
    CONSTRAINT [FK_CASE_TAG_RELATIONSHIP_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID]),
    CONSTRAINT [FK_CASE_TAG_RELATIONSHIP_CASE_TAG] FOREIGN KEY ([CASE_TAG_ID]) REFERENCES [dbo].[CASE_TAG] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件-案件標籤關聯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TAG_RELATIONSHIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件標籤代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TAG_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'CASE_TAG_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TAG_RELATIONSHIP', @level2type = N'COLUMN', @level2name = N'CASE_ID';

