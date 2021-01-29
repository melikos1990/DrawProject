CREATE TABLE [dbo].[BATCH_RECORD] (
    [NAME]                    NVARCHAR (50) NOT NULL,
    [RECENT_EXECUTE_DATETIME] DATETIME      NULL,
    [RECENT_FINISH_DATETIME]  DATETIME      NULL,
    [EXECUTE_COUNT]           INT           NOT NULL,
    CONSTRAINT [PK_BATCH_RECORD] PRIMARY KEY CLUSTERED ([NAME] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BATCH執行紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BATCH_RECORD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BATCH_RECORD', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上一次執行時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BATCH_RECORD', @level2type = N'COLUMN', @level2name = N'RECENT_EXECUTE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上一次完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BATCH_RECORD', @level2type = N'COLUMN', @level2name = N'RECENT_FINISH_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'執行次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BATCH_RECORD', @level2type = N'COLUMN', @level2name = N'EXECUTE_COUNT';

