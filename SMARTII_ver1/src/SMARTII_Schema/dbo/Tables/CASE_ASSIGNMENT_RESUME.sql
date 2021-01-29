CREATE TABLE [dbo].[CASE_ASSIGNMENT_RESUME] (
    [ID]                        INT            IDENTITY (1, 1) NOT NULL,
    [CASE_ID]                   CHAR (14)      NOT NULL,
    [CASE_ASSIGNMENT_ID]        INT            NOT NULL,
    [CONTENT]                   NVARCHAR (MAX) NULL,
    [CASE_ASSIGNMENT_TYPE]      TINYINT        NULL,
    [CREATE_DATETIME]           DATETIME       NOT NULL,
    [CREATE_USERNAME]           NVARCHAR (20)  NOT NULL,
    [CREATE_NODE_ID]            INT            NULL,
    [CREATE_NODE_NAME]          NVARCHAR (512) NULL,
    [CREATE_ORGANIZIATION_TYPE] TINYINT        NULL,
    [IS_REPLY]                  BIT            CONSTRAINT [DF_CASE_ASSIGNMENT_RESUME_IS_REPLY] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT_RESUME] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷程-派工異動紀錄(USER看)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'派工序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CASE_ASSIGNMENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異動內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'派工狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CASE_ASSIGNMENT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者單位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_RESUME', @level2type = N'COLUMN', @level2name = N'CREATE_NODE_ID';

