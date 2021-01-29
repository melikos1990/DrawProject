CREATE TABLE [dbo].[CASE_NOTICE] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [CASE_ID]          CHAR (14)      NOT NULL,
    [APPLY_USER_ID]    NVARCHAR (256) NOT NULL,
    [CREATE_DATETIME]  DATETIME       NOT NULL,
    [CREATE_USERNAME]  NVARCHAR (20)  NOT NULL,
    [CASE_NOTICE_TYPE] TINYINT        NOT NULL,
    CONSTRAINT [PK_CASE_NOTICE] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_NOTICE', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_NOTICE', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_NOTICE', @level2type = N'COLUMN', @level2name = N'APPLY_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_NOTICE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_NOTICE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';

