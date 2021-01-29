CREATE TABLE [dbo].[WORK_SCHEDULE] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       NOT NULL,
    [NODE_ID]           INT           NOT NULL,
    [DATE]              DATETIME      NOT NULL,
    [WORK_TYPE]         TINYINT       NOT NULL,
    [TITLE]             NVARCHAR (20) NULL,
    [CREATE_DATETIME]   DATETIME      NOT NULL,
    [CREATE_USERNAME]   NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]   DATETIME      NULL,
    [UPDATE_USERNAME]   NVARCHAR (20) NULL,
    CONSTRAINT [PK_WORK_SCHEDULE] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20191030-095947]
    ON [dbo].[WORK_SCHEDULE]([ORGANIZATION_TYPE] ASC, [NODE_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20191030-100059]
    ON [dbo].[WORK_SCHEDULE]([DATE] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特例假日主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'DATE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作型態 ( 0 : 上班 ; 1:休假)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'WORK_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'TITLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WORK_SCHEDULE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';

