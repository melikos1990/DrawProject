CREATE TABLE [dbo].[OFFICIAL_EMAIL_HISTORY] (
    [EMAIL_GROUP_ID]    INT            NOT NULL,
    [MESSAGE_ID]        NVARCHAR (250) NOT NULL,
    [DOWNLOAD_DATETIME] DATETIME       NOT NULL,
    [NODE_ID]           INT            NOT NULL,
    [ORGANIZATION_TYPE] TINYINT        NOT NULL,
    CONSTRAINT [PK_OFFICIAL_EMAIL_HISTORY] PRIMARY KEY CLUSTERED ([EMAIL_GROUP_ID] ASC, [MESSAGE_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'官網來信通知收信歷程(系統看)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_HISTORY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BU名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_HISTORY', @level2type = N'COLUMN', @level2name = N'EMAIL_GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱Mail ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_HISTORY', @level2type = N'COLUMN', @level2name = N'MESSAGE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下載時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OFFICIAL_EMAIL_HISTORY', @level2type = N'COLUMN', @level2name = N'DOWNLOAD_DATETIME';

