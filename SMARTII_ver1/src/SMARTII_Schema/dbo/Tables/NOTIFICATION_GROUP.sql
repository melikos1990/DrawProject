CREATE TABLE [dbo].[NOTIFICATION_GROUP] (
    [ID]                         INT           IDENTITY (1, 1) NOT NULL,
    [NAME]                       NVARCHAR (20) NOT NULL,
    [NODE_ID]                    INT           NOT NULL,
    [ORGANIZATION_TYPE]          TINYINT       CONSTRAINT [DF_NOTIFICATION_GROUP_ORGANIZATION_TYPE] DEFAULT ((0)) NOT NULL,
    [ITEM_ID]                    INT           NULL,
    [QUESTION_CLASSIFICATION_ID] INT           NULL,
    [CALC_MODE]                  TINYINT       NOT NULL,
    [ALERT_COUNT]                INT           CONSTRAINT [DF_NOTIFICATION_GROUP_ALERT_COUNT] DEFAULT ((0)) NOT NULL,
    [ALERT_CYCLE_DAY]            INT           NOT NULL,
    [ACTUAL_COUNT]               INT           NOT NULL,
    [IS_ARRIVE]                  BIT           CONSTRAINT [DF_NotificationGroup_IsArrive] DEFAULT ((0)) NOT NULL,
    [IS_NOTIFICATED]             BIT           CONSTRAINT [DF_NOTIFICATION_GROUP_IS_DONE] DEFAULT ((0)) NOT NULL,
    [UPDATE_USERNAME]            NVARCHAR (20) NULL,
    [CREATE_DATETIME]            DATETIME      NOT NULL,
    [CREATE_USERNAME]            NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]            DATETIME      NULL,
    CONSTRAINT [PK_NOTIFICATION_GROUP] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提醒群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ITEM_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'QUESTION_CLASSIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計算模式 (0 : 商品 ; 1:問題分類 ; 2 : 兩者都)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'CALC_MODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'示警次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ALERT_COUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'示警週期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ALERT_CYCLE_DAY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際達標數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'ACTUAL_COUNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否達標', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'IS_ARRIVE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該規則是否已通知過了', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'IS_NOTIFICATED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NOTIFICATION_GROUP', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';

