CREATE TABLE [dbo].[ENTERPRISE] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [NAME]            NVARCHAR (50) NULL,
    [CREATE_DATETIME] DATETIME      NOT NULL,
    [CREATE_USERNAME] NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME] DATETIME      NULL,
    [UPDATE_USERNAME] NVARCHAR (20) NULL,
    [IS_ENABLED]      BIT           CONSTRAINT [DF_ENTERPRISE_IS_ENABLED] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ENTERPRISE] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'集團別主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'集團別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'集團別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ENTERPRISE', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';

