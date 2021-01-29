CREATE TABLE [dbo].[NODE_JOB] (
    [ID]                INT     IDENTITY (1, 1) NOT NULL,
    [JOB_ID]            INT     NOT NULL,
    [NODE_ID]           INT     NOT NULL,
    [RIGHT_BOUNDARY]    INT     NOT NULL,
    [LEFT_BOUNDARY]     INT     NOT NULL,
    [ORGANIZATION_TYPE] TINYINT NOT NULL,
    [IDENTIFICATION_ID] INT     NULL,
    CONSTRAINT [PK_NODE_JOB] PRIMARY KEY CLUSTERED ([ID] ASC, [JOB_ID] ASC, [NODE_ID] ASC, [ORGANIZATION_TYPE] ASC),
    CONSTRAINT [FK_NODE_JOB_USER_JOB] FOREIGN KEY ([JOB_ID]) REFERENCES [dbo].[JOB] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織職稱關聯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點右邊界', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'RIGHT_BOUNDARY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點左邊界', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'LEFT_BOUNDARY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別值(BU_ID)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB', @level2type = N'COLUMN', @level2name = N'IDENTIFICATION_ID';

