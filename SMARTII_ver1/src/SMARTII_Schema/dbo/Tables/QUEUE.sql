CREATE TABLE [dbo].[QUEUE] (
    [ID]                NVARCHAR (50) NOT NULL,
    [NAME]              NVARCHAR (50) NOT NULL,
    [IS_ENABLED]        BIT           CONSTRAINT [DF_QUEUE_IS_ENABLED] DEFAULT ((1)) NOT NULL,
    [ORGANIZATION_TYPE] TINYINT       CONSTRAINT [DF_QUEUE_ORGANIZATION_TYPE] DEFAULT ((1)) NOT NULL,
    [NODE_ID]           INT           NOT NULL,
    CONSTRAINT [PK_QUEUE] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'服務Queue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QUEUE 代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QUEUE名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE', @level2type = N'COLUMN', @level2name = N'NAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用(預設:是)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE', @level2type = N'COLUMN', @level2name = N'IS_ENABLED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QUEUE', @level2type = N'COLUMN', @level2name = N'NODE_ID';

