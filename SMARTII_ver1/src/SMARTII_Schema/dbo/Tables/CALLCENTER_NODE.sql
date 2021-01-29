﻿CREATE TABLE [dbo].[CALLCENTER_NODE] (
    [NODE_ID]                INT           IDENTITY (1, 1) NOT NULL,
    [ORGANIZATION_TYPE]      TINYINT       CONSTRAINT [DF_CC_NODE_ORGANIZATION_TYPE] DEFAULT ((1)) NOT NULL,
    [LEFT_BOUNDARY]          INT           NOT NULL,
    [RIGHT_BOUNDARY]         INT           NOT NULL,
    [NAME]                   NVARCHAR (20) NOT NULL,
    [CREATE_DATETIME]        DATETIME      NOT NULL,
    [CREATE_USERNAME]        NVARCHAR (20) NOT NULL,
    [UPDATE_DATETIME]        DATETIME      NULL,
    [UPDATE_USERNAME]        NVARCHAR (20) NULL,
    [DEPTH_LEVEL]            INT           CONSTRAINT [DF_CC_NODE_DEPTH_LEVEL] DEFAULT ((0)) NOT NULL,
    [NODE_TYPE]              INT           NULL,
    [NODE_TYPE_KEY]          NVARCHAR (20) NULL,
    [IS_ENABLED]             BIT           CONSTRAINT [DF_CC_NODE_IS_ENABLED] DEFAULT ((1)) NOT NULL,
    [PARENT_PATH]            NVARCHAR (20) NULL,
    [PARENT_ID]              INT           NULL,
    [WORK_PROCESS_TYPE]      TINYINT       CONSTRAINT [DF_CALLCENTER_NODE_WORK_PROCESS_TYPE] DEFAULT ((0)) NOT NULL,
    [CALLCENTER_ID]          INT           NULL,
    [NODE_KEY]               CHAR (3)      NULL,
    [IS_WORK_PROCESS_NOTICE] BIT           CONSTRAINT [DF_CALLCENTER_NODE_IS)WORK_PROCESS_NOTICE] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CC_NODE_1] PRIMARY KEY CLUSTERED ([NODE_ID] ASC, [ORGANIZATION_TYPE] ASC),
    CONSTRAINT [FK_CALLCENTER_NODE_CALLCENTER_NODE] FOREIGN KEY ([CALLCENTER_ID], [ORGANIZATION_TYPE]) REFERENCES [dbo].[CALLCENTER_NODE] ([NODE_ID], [ORGANIZATION_TYPE]),
    CONSTRAINT [FK_CC_NODE_ORGANIZATION_NODE_DEFINITION] FOREIGN KEY ([ORGANIZATION_TYPE], [NODE_TYPE]) REFERENCES [dbo].[ORGANIZATION_NODE_DEFINITION] ([ORGANIZATION_TYPE], [ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客服中心組織節點主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'左邊界', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'LEFT_BOUNDARY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'右邊界', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'RIGHT_BOUNDARY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織深度(高=>低)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'DEPTH_LEVEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點定義', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'NODE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點識別值(根節點、企業別、門市)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'NODE_TYPE_KEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用(預設:是)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父節點路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'PARENT_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'PARENT_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作模式(單人/偕同)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'WORK_PROCESS_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客服中心代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'CALLCENTER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'企業代號 (識別值3碼)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'NODE_KEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否處理通知', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CALLCENTER_NODE', @level2type = N'COLUMN', @level2name = N'IS_WORK_PROCESS_NOTICE';

