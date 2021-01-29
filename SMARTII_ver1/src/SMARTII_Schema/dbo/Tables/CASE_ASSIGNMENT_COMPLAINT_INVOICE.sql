CREATE TABLE [dbo].[CASE_ASSIGNMENT_COMPLAINT_INVOICE] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [INVOICE_ID]             CHAR (13)       NOT NULL,
    [CASE_ID]                CHAR (14)       NOT NULL,
    [NODE_ID]                INT             NOT NULL,
    [ORGANIZATION_TYPE]      TINYINT         NOT NULL,
    [INVOICE_TYPE]           NVARCHAR (2)    CONSTRAINT [DF_CASE_ASSIGNMENT_COMPLAINT_INVOICE_INVOICE_TYPE] DEFAULT ((0)) NOT NULL,
    [IS_RECALL]              BIT             CONSTRAINT [DF_CASE_ASSIGNMENT_COMPLAINT_INVOICE_IS_RECALL] DEFAULT ((0)) NOT NULL,
    [FILE_PATH]              NVARCHAR (2048) NULL,
    [CONTENT]                NVARCHAR (1024) NULL,
    [NOTIFICATION_BEHAVIORS] NVARCHAR (1024) NULL,
    [NOTICE_USERs]           NVARCHAR (256)  NULL,
    [NOTICE_DATETIME]        DATETIME        NULL,
    [CREATE_DATETIME]        DATETIME        NOT NULL,
    [CREATE_USERNAME]        NVARCHAR (20)   NOT NULL,
    [EML_FILE_PATH]          NVARCHAR (256)  NULL,
    [TYPE]                   TINYINT         CONSTRAINT [DF_CASE_ASSIGNMENT_COMPLAINT_INVOICE_TYPE] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT_COMPLAINT_INVOICE] PRIMARY KEY CLUSTERED ([ID] ASC, [INVOICE_ID] ASC),
    CONSTRAINT [FK_CASE_ASSIGNMENT_COMPLAINT_INVOICE_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷程-反應單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'反應類型(A: 營業 B:行銷 C:其他) + BU 代碼(3碼) + yyMMdd(西元) + 3碼流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'INVOICE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'反應類別 (來自系統參數)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'INVOICE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否回電', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'IS_RECALL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檔案路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知行為', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_BEHAVIORS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'NOTICE_USERs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'NOTICE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'反應單通知EMAIL備份路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'反應單狀態 : 0 : 已開立 ; 1 : 已發送', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_COMPLAINT_INVOICE', @level2type = N'COLUMN', @level2name = N'TYPE';

