CREATE TABLE [dbo].[BILL_BOARD] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [ACTIVE_DATE_START] DATETIME        NOT NULL,
    [ACTIVE_DATE_END]   DATETIME        NOT NULL,
    [CONTENT]           NVARCHAR (1024) NULL,
    [FILE_PATH]         NVARCHAR (MAX)  NULL,
    [TITLE]             NVARCHAR (256)  NULL,
    [WARNING_TYPE]      TINYINT         NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20)   NOT NULL,
    [CREATE_USER_ID]    NVARCHAR (256)  NOT NULL,
    [CREATE_DATETIME]   DATETIME        NOT NULL,
    [UPDATE_USERNAME]   NVARCHAR (20)   NULL,
    [UPDATE_USER_ID]    NVARCHAR (256)  NULL,
    [UPDATE_DATETIME]   DATETIME        NULL,
    [USER_IDs]          NVARCHAR (MAX)  NULL,
    [IS_NOTIFICATED]    BIT             CONSTRAINT [DF_BILL_BOARD_IS_NOTIFICATED] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BILL_BOARD] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'佈告欄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公告生效日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'ACTIVE_DATE_START';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公告停用日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'ACTIVE_DATE_END';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公告內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'副檔路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主旨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'緊急程度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'WARNING_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'CREATE_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'UPDATE_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'可視人員清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'USER_IDs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否已執行通知', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BILL_BOARD', @level2type = N'COLUMN', @level2name = N'IS_NOTIFICATED';

