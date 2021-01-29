CREATE TABLE [dbo].[NOTIFICATION_GROUP_RESUME] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [GROUP_ID]        INT            NOT NULL,
    [NODE_NAME]       NVARCHAR (50)  NOT NULL,
    [NODE_ID]         INT            NOT NULL,
    [TARGET]          NVARCHAR (256) NULL,
    [CONTENT]         NVARCHAR (MAX) NULL,
    [TYPE]            TINYINT        CONSTRAINT [DF_NOTIFICATION_GROUP_RESUME_TYPE] DEFAULT ((0)) NOT NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    [EML_FILE_PATH]   NVARCHAR (256) NULL,
    CONSTRAINT [PK_NOTIFICATION_GROUP_RESUME] PRIMARY KEY CLUSTERED ([ID] ASC, [GROUP_ID] ASC),
    CONSTRAINT [FK_NOTIFICATION_GROUP_RESUME_NOTIFICATION_GROUP1] FOREIGN KEY ([GROUP_ID]) REFERENCES [dbo].[NOTIFICATION_GROUP] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒群組發送歷程', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'TARGET';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發送內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發送結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒通知EMAIL備份路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_RESUME', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';

