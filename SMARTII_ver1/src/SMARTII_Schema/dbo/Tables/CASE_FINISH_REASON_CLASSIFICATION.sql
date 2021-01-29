CREATE TABLE [dbo].[CASE_FINISH_REASON_CLASSIFICATION] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [NODE_ID]           INT           NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       NOT NULL,
    [ORDER]             INT           CONSTRAINT [DF_CASE_FINISH_REASON_CLASSIFICATION_ORDER] DEFAULT ((0)) NOT NULL,
    [TITLE]             NVARCHAR (20) NOT NULL,
    [IS_ENABLED]        BIT           NOT NULL,
    [IS_MULTIPLE]       BIT           CONSTRAINT [DF_CASE_FINISH_REASON_CLASSIFICATION_IS_MULTIPLE] DEFAULT ((0)) NOT NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    [KEY]               NVARCHAR (10) NULL,
    [IS_REQUIRED]       BIT           NOT NULL,
    CONSTRAINT [PK_CASE_FINISH_REASON_CLASSIFICATION] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案處置分類主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ORDER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否多選', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'IS_MULTIPLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否必填', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'IS_REQUIRED';

