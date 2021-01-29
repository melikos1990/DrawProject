CREATE TABLE [dbo].[CASE_ASSIGNMENT_HISTORY] (
    [ID]                   INT            IDENTITY (1, 1) NOT NULL,
    [CASE_ID]              CHAR (14)      NOT NULL,
    [ASSIGNMENT_ID]        INT            NOT NULL,
    [CONTENT]              NVARCHAR (MAX) NULL,
    [CASE_ASSIGNMENT_TYPE] TINYINT        NOT NULL,
    [CREATE_DATETIME]      DATETIME       NOT NULL,
    [CREATE_USERNAME]      NVARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT_HISTORY] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷程-派工異動紀錄(系統看)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'ASSIGNMENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異動內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案狀態 (0 : 立案 ; 1 : 銷案)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'CASE_ASSIGNMENT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_HISTORY', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';

