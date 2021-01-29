CREATE TABLE [dbo].[USER_ROLE] (
    [ROLE_ID] INT            NOT NULL,
    [USER_ID] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_USER_ROLE] PRIMARY KEY CLUSTERED ([ROLE_ID] ASC, [USER_ID] ASC),
    CONSTRAINT [FK_USER_ROLE_ROLE] FOREIGN KEY ([ROLE_ID]) REFERENCES [dbo].[ROLE] ([ID]),
    CONSTRAINT [FK_USER_ROLE_USER] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USER] ([USER_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員操作權限關聯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_ROLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'操作權限代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_ROLE', @level2type = N'COLUMN', @level2name = N'ROLE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER_ROLE', @level2type = N'COLUMN', @level2name = N'USER_ID';

