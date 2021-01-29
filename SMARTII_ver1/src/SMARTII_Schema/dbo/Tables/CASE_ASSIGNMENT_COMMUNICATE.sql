CREATE TABLE [dbo].[CASE_ASSIGNMENT_COMMUNICATE] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [CASE_ID]                CHAR (14)       NOT NULL,
    [NODE_ID]                INT             NOT NULL,
    [ORGANIZATION_TYPE]      TINYINT         NOT NULL,
    [NOTIFICATION_BEHAVIORS] NVARCHAR (1024) NULL,
    [NOTICE_DATETIME]        DATETIME        NULL,
    [NOTICE_USERs]           NVARCHAR (256)  NULL,
    [CONTENT]                NVARCHAR (2048) NULL,
    [CREATE_DATETIME]        DATETIME        NOT NULL,
    [CREATE_USERNAME]        NVARCHAR (20)   NOT NULL,
    [EML_FILE_PATH]          NVARCHAR (256)  NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT_COMMUNICATE] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CASE_ASSIGNMENT_COMMUNICATE_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一般通知EMAIL備份路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMMUNICATE', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';

