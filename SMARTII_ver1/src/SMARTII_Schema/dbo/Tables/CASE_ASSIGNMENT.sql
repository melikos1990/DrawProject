CREATE TABLE [dbo].[CASE_ASSIGNMENT] (
    [ASSIGNMENT_ID]            INT             NOT NULL,
    [CASE_ID]                  CHAR (14)       NOT NULL,
    [NODE_ID]                  INT             NOT NULL,
    [ORGANIZATION_TYPE]        TINYINT         NOT NULL,
    [CONTENT]                  NVARCHAR (1024) NULL,
    [FILE_PATH]                NVARCHAR (2048) NULL,
    [FINISH_CONTENT]           NVARCHAR (1024) NULL,
    [FINISH_FILE_PATH]         NVARCHAR (2048) NULL,
    [FINISH_DATETIME]          DATETIME        NULL,
    [FINISH_USERNAME]          NVARCHAR (20)   NULL,
    [FINISH_NODE_ID]           INT             NULL,
    [FINISH_NODE_NAME]         NVARCHAR (512)  NULL,
    [FINISH_ORGANIZATION_TYPE] TINYINT         NULL,
    [NOTIFICATION_BEHAVIORS]   NVARCHAR (1024) NULL,
    [NOTICE_DATETIME]          DATETIME        NULL,
    [NOTICE_USERs]             NVARCHAR (256)  NULL,
    [CASE_ASSIGNMENT_TYPE]     TINYINT         NOT NULL,
    [CREATE_DATETIME]          DATETIME        NOT NULL,
    [CREATE_USERNAME]          NVARCHAR (20)   NOT NULL,
    [UPDATE_DATETIME]          DATETIME        NULL,
    [UPDATE_USERNAME]          NVARCHAR (20)   NULL,
    [EML_FILE_PATH]            NVARCHAR (256)  NULL,
    [REJECT_TYPE]              TINYINT         CONSTRAINT [DF_CASE_ASSIGNMENT_REJECT_TYPE] DEFAULT ((0)) NOT NULL,
    [REJECT_REASON]            NVARCHAR (256)  NULL,
    [RECALL_TIMEs]             INT             CONSTRAINT [DF_CASE_ASSIGNMENT_RECALL_TIMES] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT] PRIMARY KEY CLUSTERED ([ASSIGNMENT_ID] ASC, [CASE_ID] ASC),
    CONSTRAINT [FK_CASE_ASSIGNMENT_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷程-派工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'ASSIGNMENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派附件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FINISH_CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案附件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FINISH_FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FINISH_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FINISH_NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案組織 (單位父節點)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'FINISH_NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知行為', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_BEHAVIORS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'NOTICE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'NOTICE_USERs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷案狀態 (0 : 已派工 ; 1 : 處理完成 ; 2 : 銷案)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'CASE_ASSIGNMENT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'派工通知EMAIL備份路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 : 無駁回 ; 1 : 資料重填 ; 2 : 重新處理', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'REJECT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'駁回原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT', @level2type = N'COLUMN', @level2name = N'REJECT_REASON';

