CREATE TABLE [dbo].[NOTIFICATION_GROUP_USER] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [GROUP_ID]              INT            NOT NULL,
    [USER_ID]               NVARCHAR (256) NULL,
    [USER_NAME]             NVARCHAR (256) NULL,
    [NODE_NAME]             NVARCHAR (20)  NULL,
    [NODE_ID]               INT            NULL,
    [ORGANIZATION_TYPE]     TINYINT        NULL,
    [JOB_ID]                INT            NULL,
    [JOB_NAME]              NVARCHAR (20)  NULL,
    [NOTIFICATION_BEHAVIOR] NVARCHAR (20)  NOT NULL,
    [NOTIFICATION_KIND]     NVARCHAR (20)  NULL,
    [NOTIFICATION_REMARK]   NVARCHAR (50)  NULL,
    [BU_ID]                 INT            NULL,
    [BU_NAME]               NVARCHAR (50)  NULL,
    [UNIT_TYPE]             TINYINT        CONSTRAINT [DF_NOTIFICATION_USER_IS_ORGANIZATION] DEFAULT ((0)) NOT NULL,
    [ADDRESS]               NVARCHAR (256) NULL,
    [MOBILE]                NVARCHAR (256) NULL,
    [TELEPHONE]             NVARCHAR (256) NULL,
    [TELEPHONE_BAK]         NVARCHAR (256) NULL,
    [GENDER]                TINYINT        NULL,
    [EMAIL]                 NVARCHAR (256) NULL,
    CONSTRAINT [PK_NOTIFICATION_GROUP_USER] PRIMARY KEY CLUSTERED ([ID] ASC, [GROUP_ID] ASC),
    CONSTRAINT [FK_NOTIFICATION_GROUP_USER_NOTIFICATION_GROUP] FOREIGN KEY ([GROUP_ID]) REFERENCES [dbo].[NOTIFICATION_GROUP] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒群組人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'USER_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'JOB_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒方式 (EX : Mobile)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_BEHAVIOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒種類 (EX : jpush)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_KIND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒註記 (EX : CC/收件者/密件)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_REMARK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'BU_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'BU_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為組織內人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'UNIT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手機', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'MOBILE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE_BAK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'性別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'GENDER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_USER', @level2type = N'COLUMN', @level2name = N'EMAIL';

