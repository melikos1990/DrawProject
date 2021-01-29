CREATE TABLE [dbo].[SYSTEM_PARAMETER] (
    [ID]              NVARCHAR (50)  NOT NULL,
    [KEY]             NVARCHAR (50)  NOT NULL,
    [VALUE]           NVARCHAR (MAX) NULL,
    [TEXT]            NVARCHAR (MAX) NOT NULL,
    [CREATE_DATETIME] DATETIME       NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20)  NOT NULL,
    [UPDATE_USERNAME] NVARCHAR (20)  NULL,
    [UPDATE_DATETIME] DATETIME       NULL,
    [ACTIVE_DATETIME] DATETIME       NULL,
    [NEXT_VALUE]      NVARCHAR (MAX) NULL,
    [IS_UNDELETABLE]  BIT            CONSTRAINT [DF_SYSTEM_PARAMETER_IS_PRESIST] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SYSTEM_PARAMETER] PRIMARY KEY CLUSTERED ([ID] ASC, [KEY] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統參數主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'KEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'VALUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示文字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'TEXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生效日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'ACTIVE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下次生效值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'NEXT_VALUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'能不能被使用者刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SYSTEM_PARAMETER', @level2type = N'COLUMN', @level2name = N'IS_UNDELETABLE';

