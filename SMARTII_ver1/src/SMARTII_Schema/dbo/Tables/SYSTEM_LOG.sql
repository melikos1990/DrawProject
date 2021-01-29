CREATE TABLE [dbo].[SYSTEM_LOG] (
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [FEATURE_NAME]        NVARCHAR (50) NOT NULL,
    [FEATURE_TAG]         NVARCHAR (50) NOT NULL,
    [CONTENT]             VARCHAR (MAX) NULL,
    [CREATE_DATETIME]     DATETIME      NOT NULL,
    [CREATE_USERNAME]     NVARCHAR (50) NULL,
    [CREATE_USER_ACCOUNT] NVARCHAR (50) NULL,
    [OPERATOR]            INT           NOT NULL,
    CONSTRAINT [PK_SYSTEM_LOG] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_SYSTEM_LOG_23_2107154552__K2D_1_3_4_5_6_7_8]
    ON [dbo].[SYSTEM_LOG]([FEATURE_NAME] DESC)
    INCLUDE([ID], [FEATURE_TAG], [CONTENT], [CREATE_DATETIME], [CREATE_USERNAME], [CREATE_USER_ACCOUNT], [OPERATOR]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_SYSTEM_LOG_23_2107154552__K8]
    ON [dbo].[SYSTEM_LOG]([OPERATOR] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統操作軌跡', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'FEATURE_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'FEATURE_TAG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'CREATE_USER_ACCOUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'操作權限(JSON)
ADD/DELETE/UPDATE/READ/ADMIN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_LOG', @level2type = N'COLUMN', @level2name = N'OPERATOR';

