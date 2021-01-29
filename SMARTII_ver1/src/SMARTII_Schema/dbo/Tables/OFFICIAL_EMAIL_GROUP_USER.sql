CREATE TABLE [dbo].[OFFICIAL_EMAIL_GROUP_USER] (
    [MAIL_GROUP_ID] INT            NOT NULL,
    [USER_ID]       NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_MAIL_GROUP_USER] PRIMARY KEY CLUSTERED ([MAIL_GROUP_ID] ASC, [USER_ID] ASC),
    CONSTRAINT [FK_MAIL_GROUP_USER_MAIL_GROUP] FOREIGN KEY ([MAIL_GROUP_ID]) REFERENCES [dbo].[OFFICIAL_EMAIL_GROUP] ([ID]),
    CONSTRAINT [FK_MAIL_GROUP_USER_USER] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USER] ([USER_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'官網來信通知人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'官網來信提醒ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP_USER', @level2type = N'COLUMN', @level2name = N'MAIL_GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_GROUP_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';

