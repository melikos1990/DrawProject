CREATE TABLE [dbo].[ORGANIZATION_NODE_DEFINITION] (
    [ORGANIZATION_TYPE]   TINYINT       NOT NULL,
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [NAME]                NVARCHAR (20) NOT NULL,
    [CREATE_DATETIME]     DATETIME      NOT NULL,
    [CREATE_USERNAME]     NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]     DATETIME      NULL,
    [UPDATE_USERNAME]     NVARCHAR (20) NULL,
    [IS_ENABLED]          BIT           NOT NULL,
    [IDENTIFICATION_ID]   INT           NULL,
    [IDENTIFICATION_NAME] NVARCHAR (50) NULL,
    [LEVEL]               INT           CONSTRAINT [DF_ORGANIZATION_NODE_DEFINITION_LEVEL] DEFAULT ((0)) NOT NULL,
    [KEY]                 NVARCHAR (20) NULL,
    CONSTRAINT [PK_ORGANIZATION_NODE_DEFINITION_1] PRIMARY KEY CLUSTERED ([ORGANIZATION_TYPE] ASC, [ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織階層定義檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (CC / HEADQUARTER / VENDOR)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點定義代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'定義名稱(EX : 區/課/門市)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'IDENTIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'IDENTIFICATION_NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'階層', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'LEVEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織定義KEY (EX:BU/STORE)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ORGANIZATION_NODE_DEFINITION', @level2type = N'COLUMN', @level2name = N'KEY';

