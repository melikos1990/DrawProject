CREATE TABLE [dbo].[KM_DATA] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [CLASSIFICATION_ID] INT             NOT NULL,
    [TITLE]             NVARCHAR (30)   NOT NULL,
    [CONTENT]           NVARCHAR (2048) NOT NULL,
    [CREATE_DATETIME]   DATETIME        NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20)   NOT NULL,
    [UPDATE_DATETIME]   DATETIME        NULL,
    [UPDATE_USERNAME]   NVARCHAR (20)   NULL,
    [FILE_PATH]         NVARCHAR (2048) NULL,
    CONSTRAINT [PK_KM_DATA] PRIMARY KEY CLUSTERED ([ID] ASC, [CLASSIFICATION_ID] ASC),
    CONSTRAINT [FK_KM_DATA_KM_CLASSIFICATION] FOREIGN KEY ([CLASSIFICATION_ID]) REFERENCES [dbo].[KM_CLASSIFICATION] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'常見問題討論內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'CLASSIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主旨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KM內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'常見問題附件放置路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_DATA', @level2type = N'COLUMN', @level2name = N'FILE_PATH';

