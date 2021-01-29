CREATE TABLE [dbo].[PERSONAL_NOTIFICATION] (
    [ID]                         INT             IDENTITY (1, 1) NOT NULL,
    [USER_ID]                    NVARCHAR (256)  NOT NULL,
    [CONTENT]                    NVARCHAR (4000) NULL,
    [CREATE_USERNAME]            NVARCHAR (20)   NOT NULL,
    [CREATE_DATETIME]            DATETIME        NOT NULL,
    [PERSONAL_NOTIFICATION_TYPE] TINYINT         NOT NULL,
    [EXTEND]                     NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_PERSONAL_NOTIFICATION] PRIMARY KEY CLUSTERED ([USER_ID] ASC, [ID] ASC),
    CONSTRAINT [FK_PERSONAL_NOTIFICATION_USER] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USER] ([USER_ID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_PERSONAL_NOTIFICATION_23_1949966023__K5D_1_2_3_4_6_7]
    ON [dbo].[PERSONAL_NOTIFICATION]([CREATE_DATETIME] DESC)
    INCLUDE([ID], [USER_ID], [CONTENT], [CREATE_USERNAME], [PERSONAL_NOTIFICATION_TYPE], [EXTEND]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_PERSONAL_NOTIFICATION_23_1949966023__K6_K2_1_3_4_5_7]
    ON [dbo].[PERSONAL_NOTIFICATION]([PERSONAL_NOTIFICATION_TYPE] ASC, [USER_ID] ASC)
    INCLUDE([ID], [CONTENT], [CREATE_USERNAME], [CREATE_DATETIME], [EXTEND]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者相關個人提醒', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知型態 提醒群組設定=NOTIFICATION_GROUP,職代分配=
CASE_ASSIGN,
案件銷案=CASE_FINISH,
認養MAIL工單=
MAIL_ADOPT,
案件管理-案件異動=
CASE_MODIFY,
官網來信提醒=MAIL_INCOMING

', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PERSONAL_NOTIFICATION', @level2type = N'COLUMN', @level2name = N'PERSONAL_NOTIFICATION_TYPE';

