CREATE TABLE [dbo].[CALLCENTER_NODE_QUEUE] (
    [QUEUE_ID]          NVARCHAR (50) NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       CONSTRAINT [DF_CALLCENTER_NODE_QUEUE_ORGANIZATION_TYPE] DEFAULT ((1)) NOT NULL,
    [NODE_ID]           INT           NOT NULL,
    CONSTRAINT [PK_CALLCENTER_NODE_QUEUE] PRIMARY KEY CLUSTERED ([QUEUE_ID] ASC, [ORGANIZATION_TYPE] ASC, [NODE_ID] ASC),
    CONSTRAINT [FK_CALLCENTER_NODE_QUEUE_CALLCENTER_NODE] FOREIGN KEY ([NODE_ID], [ORGANIZATION_TYPE]) REFERENCES [dbo].[CALLCENTER_NODE] ([NODE_ID], [ORGANIZATION_TYPE]),
    CONSTRAINT [FK_CALLCENTER_NODE_QUEUE_QUEUE] FOREIGN KEY ([QUEUE_ID]) REFERENCES [dbo].[QUEUE] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客服中心Queue關聯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE_QUEUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進線CTI queueID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE_QUEUE', @level2type = N'COLUMN', @level2name = N'QUEUE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE_QUEUE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE_QUEUE', @level2type = N'COLUMN', @level2name = N'NODE_ID';

