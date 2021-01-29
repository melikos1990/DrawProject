CREATE TABLE [dbo].[CASE_SOURCE_USER] (
    [SOURCE_ID]         CHAR (11)      NOT NULL,
    [BU_ID]             INT            NULL,
    [BU_NAME]           NVARCHAR (20)  NULL,
    [USER_ID]           NVARCHAR (256) NULL,
    [USER_NAME]         NVARCHAR (256) NULL,
    [JOB_ID]            INT            NULL,
    [JOB_NAME]          NVARCHAR (20)  NULL,
    [STORE_NO]          NVARCHAR (20)  NULL,
    [NODE_ID]           INT            NULL,
    [NODE_NAME]         NVARCHAR (20)  NULL,
    [ORGANIZATION_TYPE] TINYINT        NULL,
    [PARENT_PATH_NAME]  NVARCHAR (256) NULL,
    [GENDER]            TINYINT        NULL,
    [ADDRESS]           NVARCHAR (256) NULL,
    [MOBILE]            NVARCHAR (256) NULL,
    [TELEPHONE]         NVARCHAR (256) NULL,
    [TELEPHONE_BAK]     NVARCHAR (256) NULL,
    [EMAIL]             NVARCHAR (256) NULL,
    [UNIT_TYPE]         TINYINT        CONSTRAINT [DF_CASE_SOURCE_USER_UNIT_TYPE] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CASE_SOURCE_USER] PRIMARY KEY CLUSTERED ([SOURCE_ID] ASC),
    CONSTRAINT [FK_CASE_SOURCE_USER_CASE_SOURCE] FOREIGN KEY ([SOURCE_ID]) REFERENCES [dbo].[CASE_SOURCE] ([SOURCE_ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件來源反應者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'SOURCE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'BU_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員企業別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'BU_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'姓名(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'USER_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'JOB_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員組織名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'性別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'GENDER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手機', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'MOBILE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'TELEPHONE_BAK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱(加密)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'EMAIL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為組織內人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE_USER', @level2type = N'COLUMN', @level2name = N'UNIT_TYPE';

