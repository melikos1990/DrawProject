CREATE TABLE [dbo].[STORE] (
    [NODE_ID]                INT             NOT NULL,
    [CODE]                   NVARCHAR (50)   NULL,
    [NAME]                   NVARCHAR (50)   NOT NULL,
    [CREATE_DATETIME]        DATETIME        NOT NULL,
    [CREATE_USERNAME]        NVARCHAR (20)   NOT NULL,
    [UPDATE_USERNAME]        NVARCHAR (20)   NULL,
    [UPDATE_DATETIME]        DATETIME        NULL,
    [J_CONTENT]              NVARCHAR (MAX)  NULL,
    [ORGANIZATION_TYPE]      TINYINT         CONSTRAINT [DF_STORE_ORGANIZATION_TYPE] DEFAULT ((0)) NOT NULL,
    [TELEPHONE]              NVARCHAR (1024) NULL,
    [ADDRESS]                NVARCHAR (1024) NULL,
    [EMAIL]                  NVARCHAR (1024) NULL,
    [STORE_OPEN_DATETIME]    DATETIME        NULL,
    [STORE_CLOSE_DATETIME]   DATETIME        NULL,
    [STORE_TYPE]             INT             NULL,
    [MEMO]                   NVARCHAR (MAX)  NULL,
    [IS_ENABLED]             BIT             NULL,
    [SERVICE_TIME]           NVARCHAR (50)   NULL,
    [OWNER_NODE_JOB_ID]      INT             NULL,
    [SUPERVISOR_NODE_JOB_ID] INT             NULL,
    CONSTRAINT [PK_STORE] PRIMARY KEY CLUSTERED ([NODE_ID] ASC, [ORGANIZATION_TYPE] ASC),
    CONSTRAINT [FK_STORE_HEADQUARTERS_NODE] FOREIGN KEY ([NODE_ID], [ORGANIZATION_TYPE]) REFERENCES [dbo].[HEADQUARTERS_NODE] ([NODE_ID], [ORGANIZATION_TYPE])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'CODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊內容JSON', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'J_CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'市話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'TELEPHONE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'ADDRESS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'信箱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'EMAIL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市營業日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'STORE_OPEN_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市歇業日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'STORE_CLOSE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'門市型態(EX: 百貨店/快閃店) 由系統參數定義', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'STORE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'MEMO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'啟用狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'STORE', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';

