CREATE TABLE [dbo].[OFFICIAL_EMAIL_GROUP] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [NODE_ID]           INT            NULL,
    [ORGANIZATION_TYPE] TINYINT        NULL,
    [MAIL_ADDRESS]      NVARCHAR (100) NOT NULL,
    [MAIL_DISPLAY_NAME] NVARCHAR (20)  NOT NULL,
    [OFFICIAL_EMAIL]    NVARCHAR (100) NOT NULL,
    [ACCOUNT]           NVARCHAR (100) NOT NULL,
    [PASSWORD]          NVARCHAR (50)  NOT NULL,
    [CREATE_DATETIME]   DATETIME       NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20)  NOT NULL,
    [UPDATE_DATETIME]   DATETIME       NULL,
    [UPDATE_USERNAME]   NVARCHAR (20)  NULL,
    [KEEP_DAY]          INT            CONSTRAINT [DF_OFFICIAL_EMAIL_GROUP_KEEP_DAY] DEFAULT ((7)) NOT NULL,
    [PROTOCOL]          TINYINT        CONSTRAINT [DF_OFFICIAL_EMAIL_GROUP_PROTOCOL] DEFAULT ((0)) NOT NULL,
    [HOSTNAME]          NVARCHAR (50)  NULL,
    [IS_ENABLED]        BIT            CONSTRAINT [DF_OFFICIAL_EMAIL_GROUP_ISDISABLED] DEFAULT ((1)) NOT NULL,
    [ALLOW_RECEIVE]     BIT            NOT NULL,
    CONSTRAINT [PK_MAIL_GROUP] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'官網來信通知群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BU信箱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'MAIL_ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源信箱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'OFFICIAL_EMAIL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'ACCOUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'PASSWORD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EMAIL 保留天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'KEEP_DAY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 : POP3 ; 1 : OFFICE365', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'PROTOCOL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'允許收信', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP', @level2type = N'COLUMN', @level2name = N'ALLOW_RECEIVE';

