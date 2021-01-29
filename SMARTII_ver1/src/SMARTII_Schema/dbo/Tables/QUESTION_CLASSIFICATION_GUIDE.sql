CREATE TABLE [dbo].[QUESTION_CLASSIFICATION_GUIDE] (
    [CLASSIFICATION_ID] INT            NOT NULL,
    [ORGANIZATION_TYPE] TINYINT        NOT NULL,
    [NODE_ID]           INT            NOT NULL,
    [CONTENT]           NVARCHAR (MAX) NOT NULL,
    [CREATE_DATETIME]   DATETIME       NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20)  NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20)  NULL,
    [UPDATE_DATETIME]   DATETIME       NULL,
    CONSTRAINT [PK_QUESTION_CLASSIFICATION_GUIDE_1] PRIMARY KEY CLUSTERED ([CLASSIFICATION_ID] ASC),
    CONSTRAINT [FK_QUESTION_CLASSIFICATION_GUIDE_QUESTION_CLASSIFICATION] FOREIGN KEY ([CLASSIFICATION_ID]) REFERENCES [dbo].[QUESTION_CLASSIFICATION] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程引導設定主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'CLASSIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提示內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION_GUIDE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';

