CREATE TABLE [dbo].[USER] (
    [USER_ID]                       NVARCHAR (256)  NOT NULL,
    [ACCOUNT]                       NVARCHAR (50)   NULL,
    [PASSWORD]                      NVARCHAR (50)   NULL,
    [NAME]                          NVARCHAR (20)   NOT NULL,
    [EMAIL]                         NVARCHAR (256)  NULL,
    [MOBILE_PUSH_ID]                NVARCHAR (200)  NULL,
    [ERROR_PASSWORD_COUNT]          INT             CONSTRAINT [DF_USER_ERROR_PASSWORD_COUNT] DEFAULT ((0)) NOT NULL,
    [LAST_CHANGE_PASSWORD_DATETIME] DATETIME        NULL,
    [PAST_PASSWORD_RECORD]          NVARCHAR (MAX)  NULL,
    [IS_AD]                         BIT             CONSTRAINT [DF_USER_IS_AD] DEFAULT ((0)) NOT NULL,
    [IS_ENABLED]                    BIT             CONSTRAINT [DF_USER_IS_ENABLED] DEFAULT ((0)) NOT NULL,
    [LOCKOUT_DATETIME]              DATETIME        NULL,
    [CREATE_DATETIME]               DATETIME        NOT NULL,
    [CREATE_USERNAME]               NVARCHAR (20)   NOT NULL,
    [UPDATE_USERNAME]               NVARCHAR (20)   NULL,
    [UPDATE_DATETIME]               DATETIME        NULL,
    [FEATURE]                       NVARCHAR (MAX)  NULL,
    [TELEPHONE]                     NVARCHAR (1024) NULL,
    [IMAGE_PATH]                    NVARCHAR (MAX)  NULL,
    [VERSION]                       DATETIME        NOT NULL,
    [EXT]                           NVARCHAR (10)   NULL,
    [PLATFORM_TYPE]                 TINYINT         NOT NULL,
    [MOBILE]                        NVARCHAR (10)   NULL,
    [ADDRESS]                       NVARCHAR (1024) NULL,
    [ACTIVE_START_DATETIME]         DATETIME        NULL,
    [ACTIVE_END_DATETIME]           DATETIME        NULL,
    [IS_SYSTEM_USER]                BIT             CONSTRAINT [DF_USER_IS_SYSTEM_USER] DEFAULT ((1)) NOT NULL,
    [MEMBER_ID]                     NVARCHAR (50)   NULL,
    CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED ([USER_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K11D_K1_2_3_4_5_6_7_8_9_10_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28]
    ON [dbo].[USER]([IS_ENABLED] DESC, [USER_ID] ASC)
    INCLUDE([ACCOUNT], [PASSWORD], [NAME], [EMAIL], [MOBILE_PUSH_ID], [ERROR_PASSWORD_COUNT], [LAST_CHANGE_PASSWORD_DATETIME], [PAST_PASSWORD_RECORD], [IS_AD], [LOCKOUT_DATETIME], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_USERNAME], [UPDATE_DATETIME], [FEATURE], [TELEPHONE], [IMAGE_PATH], [VERSION], [EXT], [PLATFORM_TYPE], [MOBILE], [ADDRESS], [ACTIVE_START_DATETIME], [ACTIVE_END_DATETIME], [IS_SYSTEM_USER], [MEMBER_ID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K4D_K1_2_3_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28]
    ON [dbo].[USER]([NAME] DESC, [USER_ID] ASC)
    INCLUDE([ACCOUNT], [PASSWORD], [EMAIL], [MOBILE_PUSH_ID], [ERROR_PASSWORD_COUNT], [LAST_CHANGE_PASSWORD_DATETIME], [PAST_PASSWORD_RECORD], [IS_AD], [IS_ENABLED], [LOCKOUT_DATETIME], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_USERNAME], [UPDATE_DATETIME], [FEATURE], [TELEPHONE], [IMAGE_PATH], [VERSION], [EXT], [PLATFORM_TYPE], [MOBILE], [ADDRESS], [ACTIVE_START_DATETIME], [ACTIVE_END_DATETIME], [IS_SYSTEM_USER], [MEMBER_ID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K11]
    ON [dbo].[USER]([IS_ENABLED] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K10_K1_2_3_4_5_6_7_8_9_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28]
    ON [dbo].[USER]([IS_AD] ASC, [USER_ID] ASC)
    INCLUDE([ACCOUNT], [PASSWORD], [NAME], [EMAIL], [MOBILE_PUSH_ID], [ERROR_PASSWORD_COUNT], [LAST_CHANGE_PASSWORD_DATETIME], [PAST_PASSWORD_RECORD], [IS_ENABLED], [LOCKOUT_DATETIME], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_USERNAME], [UPDATE_DATETIME], [FEATURE], [TELEPHONE], [IMAGE_PATH], [VERSION], [EXT], [PLATFORM_TYPE], [MOBILE], [ADDRESS], [ACTIVE_START_DATETIME], [ACTIVE_END_DATETIME], [IS_SYSTEM_USER], [MEMBER_ID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K27_K1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_28]
    ON [dbo].[USER]([IS_SYSTEM_USER] ASC, [USER_ID] ASC)
    INCLUDE([ACCOUNT], [PASSWORD], [NAME], [EMAIL], [MOBILE_PUSH_ID], [ERROR_PASSWORD_COUNT], [LAST_CHANGE_PASSWORD_DATETIME], [PAST_PASSWORD_RECORD], [IS_AD], [IS_ENABLED], [LOCKOUT_DATETIME], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_USERNAME], [UPDATE_DATETIME], [FEATURE], [TELEPHONE], [IMAGE_PATH], [VERSION], [EXT], [PLATFORM_TYPE], [MOBILE], [ADDRESS], [ACTIVE_START_DATETIME], [ACTIVE_END_DATETIME], [MEMBER_ID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K2D_K1_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28]
    ON [dbo].[USER]([ACCOUNT] DESC, [USER_ID] ASC)
    INCLUDE([PASSWORD], [NAME], [EMAIL], [MOBILE_PUSH_ID], [ERROR_PASSWORD_COUNT], [LAST_CHANGE_PASSWORD_DATETIME], [PAST_PASSWORD_RECORD], [IS_AD], [IS_ENABLED], [LOCKOUT_DATETIME], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_USERNAME], [UPDATE_DATETIME], [FEATURE], [TELEPHONE], [IMAGE_PATH], [VERSION], [EXT], [PLATFORM_TYPE], [MOBILE], [ADDRESS], [ACTIVE_START_DATETIME], [ACTIVE_END_DATETIME], [IS_SYSTEM_USER], [MEMBER_ID]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K27]
    ON [dbo].[USER]([IS_SYSTEM_USER] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K10]
    ON [dbo].[USER]([IS_AD] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K4]
    ON [dbo].[USER]([NAME] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_USER_23_1595868752__K2]
    ON [dbo].[USER]([ACCOUNT] ASC);


GO
CREATE STATISTICS [_dta_stat_1595868752_1_11]
    ON [dbo].[USER]([USER_ID], [IS_ENABLED]);


GO
CREATE STATISTICS [_dta_stat_1595868752_11_2]
    ON [dbo].[USER]([IS_ENABLED], [ACCOUNT]);


GO
CREATE STATISTICS [_dta_stat_1595868752_11_4]
    ON [dbo].[USER]([IS_ENABLED], [NAME]);


GO
CREATE STATISTICS [_dta_stat_1595868752_1_2]
    ON [dbo].[USER]([USER_ID], [ACCOUNT]);


GO
CREATE STATISTICS [_dta_stat_1595868752_1_27]
    ON [dbo].[USER]([USER_ID], [IS_SYSTEM_USER]);


GO
CREATE STATISTICS [_dta_stat_1595868752_11_27_1]
    ON [dbo].[USER]([IS_ENABLED], [IS_SYSTEM_USER], [USER_ID]);


GO
CREATE STATISTICS [_dta_stat_1595868752_1_10]
    ON [dbo].[USER]([USER_ID], [IS_AD]);


GO
CREATE STATISTICS [_dta_stat_1595868752_11_10]
    ON [dbo].[USER]([IS_ENABLED], [IS_AD]);


GO
CREATE STATISTICS [_dta_stat_1595868752_4_1_27]
    ON [dbo].[USER]([NAME], [USER_ID], [IS_SYSTEM_USER]);


GO
CREATE STATISTICS [_dta_stat_1595868752_27_1_2]
    ON [dbo].[USER]([IS_SYSTEM_USER], [USER_ID], [ACCOUNT]);


GO
CREATE STATISTICS [_dta_stat_1595868752_11_1_4]
    ON [dbo].[USER]([IS_ENABLED], [USER_ID], [NAME]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'ACCOUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'PASSWORD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者姓名(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'EMAIL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'推播TOKEN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'MOBILE_PUSH_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'登入錯誤次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'ERROR_PASSWORD_COUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後一次更改密碼的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'LAST_CHANGE_PASSWORD_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'過去輸入過的密碼紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'PAST_PASSWORD_RECORD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為AD人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'IS_AD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'LOCKOUT_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能清單(JSON)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'FEATURE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'頭貼路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'IMAGE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'VERSION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'值機分機號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'EXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平台型態 (0 : Web ; 1 : Android ; 2:IOS)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'PLATFORM_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手機號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'MOBILE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通訊地址(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生效日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'ACTIVE_START_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生效日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'ACTIVE_END_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為系統使用者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'IS_SYSTEM_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會員編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'USER', @level2type = N'COLUMN', @level2name = N'MEMBER_ID';

