CREATE TABLE [dbo].[CASE_CONCAT_USER] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [CASE_ID]               CHAR (14)      NOT NULL,
    [USER_ID]               NVARCHAR (256) NULL,
    [USER_NAME]             NVARCHAR (256) NULL,
    [NODE_NAME]             NVARCHAR (20)  NULL,
    [NODE_ID]               INT            NULL,
    [JOB_ID]                INT            NULL,
    [JOB_NAME]              NVARCHAR (20)  NULL,
    [STORE_NO]              NVARCHAR (20)  NULL,
    [BU_ID]                 INT            NULL,
    [BU_NAME]               NVARCHAR (50)  NULL,
    [ORGANIZATION_TYPE]     TINYINT        NULL,
    [PARENT_PATH_NAME]      NVARCHAR (256) NULL,
    [NOTIFICATION_BEHAVIOR] TINYINT        NOT NULL,
    [NOTIFICATION_KIND]     NVARCHAR (20)  NULL,
    [NOTIFICATION_REMARK]   NVARCHAR (50)  NULL,
    [ADDRESS]               NVARCHAR (256) NULL,
    [MOBILE]                NVARCHAR (256) NULL,
    [TELEPHONE]             NVARCHAR (256) NULL,
    [TELEPHONE_BAK]         NVARCHAR (256) NULL,
    [GENDER]                TINYINT        NULL,
    [UNIT_TYPE]             TINYINT        CONSTRAINT [DF_CASE_CONCAT_USER_UNIT_TYPE] DEFAULT ((0)) NOT NULL,
    [EMAIL]                 NVARCHAR (256) NULL,
    CONSTRAINT [PK_CASE_CONCAT_USER] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CASE_CONCAT_USER_CASE] FOREIGN KEY ([CASE_ID]) REFERENCES [dbo].[CASE] ([CASE_ID])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_CONCAT_USER_12_301960152__K2_1_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_9987]
    ON [dbo].[CASE_CONCAT_USER]([CASE_ID] ASC)
    INCLUDE([ID], [USER_ID], [USER_NAME], [NODE_NAME], [NODE_ID], [JOB_ID], [JOB_NAME], [STORE_NO], [BU_ID], [BU_NAME], [ORGANIZATION_TYPE], [PARENT_PATH_NAME], [NOTIFICATION_BEHAVIOR], [NOTIFICATION_KIND], [NOTIFICATION_REMARK], [ADDRESS], [MOBILE], [TELEPHONE], [TELEPHONE_BAK], [GENDER], [UNIT_TYPE], [EMAIL]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_CONCAT_USER_12_301960152__K2_9987]
    ON [dbo].[CASE_CONCAT_USER]([CASE_ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件-聯絡人(反應者)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者名稱(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'USER_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'JOB_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'BU_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'BU_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒方式 (EX : Mobile)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_BEHAVIOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒種類 (EX : jpush)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_KIND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒註記 (EX : CC/收件者/密件)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_REMARK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手機', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'MOBILE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE_BAK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'性別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'GENDER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為組織內人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'UNIT_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_CONCAT_USER', @level2type = N'COLUMN', @level2name = N'EMAIL';

