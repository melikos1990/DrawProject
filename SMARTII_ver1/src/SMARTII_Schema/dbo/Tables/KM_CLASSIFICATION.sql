CREATE TABLE [dbo].[KM_CLASSIFICATION] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [PARENT_ID]         INT           NULL,
    [NAME]              NVARCHAR (20) NULL,
    [NODE_ID]           INT           NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       NOT NULL,
    CONSTRAINT [PK_KM_CLASSIFICATION] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_KM_CLASSIFICATION_KM_CLASSIFICATION] FOREIGN KEY ([PARENT_ID]) REFERENCES [dbo].[KM_CLASSIFICATION] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'常見問題討論分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父階層KM 分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'PARENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KM 分類名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KM_CLASSIFICATION', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';

