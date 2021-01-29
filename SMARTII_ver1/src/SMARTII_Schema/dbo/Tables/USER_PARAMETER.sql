CREATE TABLE [dbo].[USER_PARAMETER] (
    [USER_ID]            NVARCHAR (256) NOT NULL,
    [NAVIGATE_OF_NEWBIE] BIT            NOT NULL,
    [NOTICE_OF_WEBSITE]  BIT            NOT NULL,
    [FAVORITE_FEATURE]   NVARCHAR (MAX) NULL,
    [IMAGE_PATH]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_USER_PARAMETER] PRIMARY KEY CLUSTERED ([USER_ID] ASC),
    CONSTRAINT [FK_USER_PARAMETER_USER] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USER] ([USER_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者個人參數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否彈出功能導覽', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER', @level2type = N'COLUMN', @level2name = N'NAVIGATE_OF_NEWBIE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否官網來信提醒', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER', @level2type = N'COLUMN', @level2name = N'NOTICE_OF_WEBSITE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快速功能列', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER', @level2type = N'COLUMN', @level2name = N'FAVORITE_FEATURE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'個人大頭照附件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_PARAMETER', @level2type = N'COLUMN', @level2name = N'IMAGE_PATH';

