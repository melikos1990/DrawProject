CREATE TABLE [dbo].[QUESTION_CLASSIFICATION] (
    [NODE_ID]           INT           NOT NULL,
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [PARENT_ID]         INT           NULL,
    [NAME]              NVARCHAR (20) NULL,
    [IS_ENABLED]        BIT           NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    [ORGANIZATION_TYPE] TINYINT       NOT NULL,
    [LEVEL]             INT           NOT NULL,
    [ORDER]             INT           NOT NULL,
    [CODE]              NVARCHAR (20) NULL,
    CONSTRAINT [PK_QUESTION_CLASSIFICATION_1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_QUESTION_CLASSIFICATION_QUESTION_CLASSIFICATION] FOREIGN KEY ([PARENT_ID]) REFERENCES [dbo].[QUESTION_CLASSIFICATION] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題分類主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父階層問題分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'PARENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題分類名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'LEVEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUESTION_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ORDER';

