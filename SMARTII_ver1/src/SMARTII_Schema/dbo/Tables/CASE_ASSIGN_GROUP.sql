CREATE TABLE [dbo].[CASE_ASSIGN_GROUP] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [NAME]              NVARCHAR (20) NOT NULL,
    [NODE_ID]           INT           NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       CONSTRAINT [DF_ASSIGN_GROUP_ORGANIZATION_TYPE] DEFAULT ((0)) NOT NULL,
    [TYPE]              TINYINT       NOT NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    CONSTRAINT [PK_ASSIGN_GROUP] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件派工/客製提醒群組 主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'(流水號)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別(ex:派工群組/客製化提醒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_ASSIGN_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';

