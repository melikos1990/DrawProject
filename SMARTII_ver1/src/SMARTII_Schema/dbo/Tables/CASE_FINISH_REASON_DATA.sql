CREATE TABLE [dbo].[CASE_FINISH_REASON_DATA] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [CLASSIFICATION_ID] INT           NOT NULL,
    [TEXT]              NVARCHAR (20) NOT NULL,
    [IS_ENABLED]        BIT           NOT NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    [ORDER]             INT           CONSTRAINT [DF_CASE_FINISH_REASON_DATA_ORDER] DEFAULT ((0)) NOT NULL,
    [DEFAULT]           BIT           NULL,
    CONSTRAINT [PK_CASE_FINISH_REASON_DATA] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CASE_FINISH_REASON_DATA_CASE_FINISH_REASON_CLASSIFICATION] FOREIGN KEY ([CLASSIFICATION_ID]) REFERENCES [dbo].[CASE_FINISH_REASON_CLASSIFICATION] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案處置主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'CLASSIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示文字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'TEXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'ORDER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設勾選', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_FINISH_REASON_DATA', @level2type = N'COLUMN', @level2name = N'DEFAULT';

