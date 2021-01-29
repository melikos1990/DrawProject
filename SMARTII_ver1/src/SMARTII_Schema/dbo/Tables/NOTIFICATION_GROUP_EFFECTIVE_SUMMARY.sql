CREATE TABLE [dbo].[NOTIFICATION_GROUP_EFFECTIVE_SUMMARY] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [NOTIFICATION_GROUP_ID] INT            NOT NULL,
    [EXPECT_ARRIVE_COUNT]   INT            CONSTRAINT [DF_NOTIFICATION_SUMMARY_ARRIVE_COUNT] DEFAULT ((0)) NOT NULL,
    [ACTUAL_ARRIVE_COUNT]   INT            CONSTRAINT [DF_NOTIFICATION_EFFECTIVE_SUMMARY_EXPECT_ARRIVE_COUNT1] DEFAULT ((0)) NOT NULL,
    [CASE_IDs]              NVARCHAR (MAX) NOT NULL,
    [CREATE_DATETIME]       DATETIME       NOT NULL,
    [IS_PROCESS]            BIT            NOT NULL,
    [UPDATE_DATETIME]       DATETIME       NULL,
    [UPDATE_USERNAME]       NVARCHAR (20)  NULL,
    CONSTRAINT [PK_NOTIFICATION_GROUP_EFFECTIVE_SUMMARY] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒群組通知(達標清單)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒群組ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'NOTIFICATION_GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計達標數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'EXPECT_ARRIVE_COUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際達標數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'ACTUAL_ARRIVE_COUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'達標案件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'CASE_IDs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否已(通知/不通知)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP_EFFECTIVE_SUMMARY', @level2type = N'COLUMN', @level2name = N'IS_PROCESS';

