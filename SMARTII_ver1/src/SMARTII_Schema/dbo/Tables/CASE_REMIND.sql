CREATE TABLE [dbo].[CASE_REMIND] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [CASE_ID]               CHAR (14)       NOT NULL,
    [ASSIGNMENT_ID]         INT             NULL,
    [NODE_ID]               INT             NOT NULL,
    [ORGANIZATION_TYPE]     TINYINT         NOT NULL,
    [ACTIVE_START_DAETTIME] DATETIME        NOT NULL,
    [ACTIVE_END_DAETTIME]   DATETIME        NOT NULL,
    [CONTENT]               NVARCHAR (1024) NOT NULL,
    [USER_IDs]              NVARCHAR (MAX)  NULL,
    [IS_CONFIRM]            BIT             CONSTRAINT [DF_CASE_REMIND_IS_CONFIRM] DEFAULT ((0)) NOT NULL,
    [IS_NOTIFCATED]         BIT             CONSTRAINT [DF_CASE_REMIND_IS_NOTIFCATED] DEFAULT ((0)) NOT NULL,
    [CONFIRM_USER_ID]       NVARCHAR (256)  NULL,
    [CONFIRM_DATETIME]      DATETIME        NULL,
    [CONFIRM_USERNAME]      NVARCHAR (20)   NULL,
    [TYPE]                  TINYINT         CONSTRAINT [DF_CASE_REMIND_LEVEL] DEFAULT ((0)) NOT NULL,
    [CREATE_USER_ID]        NVARCHAR (256)  NOT NULL,
    [CREATE_USERNAME]       NVARCHAR (20)   NOT NULL,
    [CREATE_DATETIME]       DATETIME        NOT NULL,
    [UPDATE_USERNAME]       NVARCHAR (20)   NULL,
    [UPDATE_DATETIME]       DATETIME        NULL,
    CONSTRAINT [PK_CASE_NOTIFICATION_CONFIG] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CASE_NOTIFICATION_CONFIG_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件追蹤示警', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'ASSIGNMENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生效開始日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'ACTIVE_START_DAETTIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生效結束日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'ACTIVE_END_DAETTIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知/可視使用者清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'USER_IDs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否確認(執行完季)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'IS_CONFIRM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CONFIRM_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CONFIRM_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CONFIRM_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'緊急程度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CREATE_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_REMIND', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';

