CREATE TABLE [dbo].[CASE_ASSIGNMENT_USER] (
    [ID]                   INT            IDENTITY (1, 1) NOT NULL,
    [CASE_ID]              CHAR (14)      NOT NULL,
    [ASSIGNMENT_ID]        INT            NOT NULL,
    [IS_APPLY]             BIT            CONSTRAINT [DF_CASE_ASSIGNMENT_USER_IS_APPLY] DEFAULT ((0)) NOT NULL,
    [USER_ID]              NVARCHAR (256) NULL,
    [USER_NAME]            NVARCHAR (256) NULL,
    [NODE_NAME]            NVARCHAR (20)  NULL,
    [NODE_ID]              INT            NULL,
    [STORE_NO]             NVARCHAR (20)  NULL,
    [JOB_ID]               INT            NULL,
    [JOB_NAME]             NVARCHAR (20)  NULL,
    [BU_ID]                INT            NULL,
    [BU_NAME]              NVARCHAR (50)  NULL,
    [ORGANIZATION_TYPE]    TINYINT        NULL,
    [PARENT_PATH_NAME]     NVARCHAR (256) NULL,
    [UNIT_TYPE]            TINYINT        CONSTRAINT [DF_CASE_ASSIGNMENT_USER_UNIT_TYPE] DEFAULT ((0)) NOT NULL,
    [COMPLAINED_USER_TYPE] TINYINT        NOT NULL,
    CONSTRAINT [PK_CASE_ASSIGNMENT_USER] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CASE_ASSIGNMENT_USER_CASE_ASSIGNMENT] FOREIGN KEY ([ASSIGNMENT_ID], [CASE_ID]) REFERENCES [dbo].[CASE_ASSIGNMENT] ([ASSIGNMENT_ID], [CASE_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷程-派工對象(以單位為主體)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉派代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'ASSIGNMENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為銷案對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'IS_APPLY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'USER_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'NODE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'JOB_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'BU_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'BU_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為組織內人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGNMENT_USER', @level2type = N'COLUMN', @level2name = N'UNIT_TYPE';

