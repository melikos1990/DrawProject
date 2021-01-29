CREATE TABLE [dbo].[CASE_TEMPLATE] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [NODE_ID]         INT            NOT NULL,
    [CLASSIFIC_KEY]   NVARCHAR (50)  NOT NULL,
    [TITLE]           NVARCHAR (255) NOT NULL,
    [EMAIL_TITLE]     NVARCHAR (255) NULL,
    [CONTENT]         NVARCHAR (MAX) NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    [UPDATE_DATETIME] DATETIME       NULL,
    [UPDATE_USERNAME] NVARCHAR (20)  NULL,
    [IS_DEFAULT]      BIT            CONSTRAINT [DF_CASE_TEMPLATE_IS_DEFAULT] DEFAULT ((0)) NOT NULL,
    [IS_FAST_FINISH]  BIT            CONSTRAINT [DF_CASE_TEMPLATE_IS_FAST_FINISH] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CASE_TEMPLATE_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'範本主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'時機分類(MAIL、反應單、通知、結案、質檢)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'CLASSIFIC_KEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主旨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信件TITLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'EMAIL_TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設範本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'IS_DEFAULT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快速結案標註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_TEMPLATE', @level2type = N'COLUMN', @level2name = N'IS_FAST_FINISH';

