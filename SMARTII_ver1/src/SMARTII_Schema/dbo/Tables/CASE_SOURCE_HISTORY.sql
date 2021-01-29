CREATE TABLE [dbo].[CASE_SOURCE_HISTORY] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [SOURCE_ID]       CHAR (12)      NOT NULL,
    [CONTENT]         NVARCHAR (MAX) NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_CASE_SOURCE_HISTORY] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件來源異動紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY', @level2type = N'COLUMN', @level2name = N'SOURCE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異動內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_HISTORY', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';

