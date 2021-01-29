CREATE TABLE [dbo].[NODE_JOB_USER] (
    [NODE_JOB_ID]       INT            NOT NULL,
    [USER_ID]           NVARCHAR (256) NOT NULL,
    [JOB_ID]            INT            NOT NULL,
    [NODE_ID]           INT            NOT NULL,
    [ORGANIZATION_TYPE] TINYINT        NOT NULL,
    CONSTRAINT [PK_NODE_JOB_USER] PRIMARY KEY CLUSTERED ([NODE_JOB_ID] ASC, [USER_ID] ASC, [JOB_ID] ASC, [NODE_ID] ASC, [ORGANIZATION_TYPE] ASC),
    CONSTRAINT [FK_NODE_JOB_USER_NODE_JOB] FOREIGN KEY ([NODE_JOB_ID], [JOB_ID], [NODE_ID], [ORGANIZATION_TYPE]) REFERENCES [dbo].[NODE_JOB] ([ID], [JOB_ID], [NODE_ID], [ORGANIZATION_TYPE]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_NODE_JOB_USER_USER] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USER] ([USER_ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織職稱使用者關聯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER', @level2type = N'COLUMN', @level2name = N'NODE_JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER', @level2type = N'COLUMN', @level2name = N'USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'職稱代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER', @level2type = N'COLUMN', @level2name = N'JOB_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織節點代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NODE_JOB_USER', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';

