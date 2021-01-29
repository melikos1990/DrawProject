CREATE TABLE [dbo].[JOB] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [DEFINITION_ID]     INT           NOT NULL,
    [NAME]              NVARCHAR (20) NOT NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    [ORGANIZATION_TYPE] TINYINT       NOT NULL,
    [IS_ENABLED]        BIT           CONSTRAINT [DF_JOB_IS_ENABLED] DEFAULT ((1)) NOT NULL,
    [LEVEL]             INT           CONSTRAINT [DF_JOB_LEVEL] DEFAULT ((0)) NOT NULL,
    [KEY]               NVARCHAR (20) NULL,
    CONSTRAINT [PK_JOB_1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_JOB_ORGANIZATION_NODE_DEFINITION] FOREIGN KEY ([ORGANIZATION_TYPE], [DEFINITION_ID]) REFERENCES [dbo].[ORGANIZATION_NODE_DEFINITION] ([ORGANIZATION_TYPE], [ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織階層代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'DEFINITION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'定義名稱(EX : 部長/課長/店長)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'LEVEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱定義KEY (EX:OFC/OWNER)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOB', @level2type = N'COLUMN', @level2name = N'KEY';

