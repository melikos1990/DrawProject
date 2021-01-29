CREATE TABLE [dbo].[CASE_SOURCE] (
    [SOURCE_ID]               CHAR (11)       NOT NULL,
    [NODE_ID]                 INT             NOT NULL,
    [ORGANIZATION_TYPE]       TINYINT         NOT NULL,
    [IS_TWICE_CALL]           BIT             CONSTRAINT [DF_CASE_SOURCE_IS_TRAIL_NOTICE] DEFAULT ((0)) NOT NULL,
    [IS_PREVENTION]           BIT             CONSTRAINT [DF_CASE_SOURCE_IS_PREVENTION] DEFAULT ((0)) NOT NULL,
    [INCOMING_DATETIME]       DATETIME        NULL,
    [RELATION_CASE_SOURCE_ID] CHAR (11)       NULL,
    [REMARK]                  NVARCHAR (2048) NULL,
    [RELATION_CASE_IDs]       NVARCHAR (1024) NULL,
    [VOICE_ID]                NVARCHAR (30)   NULL,
    [VOICE_LOCATOR]           NVARCHAR (256)  NULL,
    [SOURCE_TYPE]             TINYINT         CONSTRAINT [DF_CASE_SOURCE_SOURCE_TYPE] DEFAULT ((0)) NOT NULL,
    [CREATE_DATETIME]         DATETIME        NOT NULL,
    [CREATE_USERNAME]         NVARCHAR (20)   NOT NULL,
    [UPDATE_DATETIME]         DATETIME        NULL,
    [UPDATE_USERNAME]         NVARCHAR (20)   NULL,
    [GROUP_ID]                INT             CONSTRAINT [DF_CASE_SOURCE_GROUP_ID] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CASE_SOURCE] PRIMARY KEY CLUSTERED ([SOURCE_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號 (yymmdd + 流水號5碼)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'SOURCE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號 (企業別)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態 (總部)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為二次來電', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'IS_TWICE_CALL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為預立案來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'IS_PREVENTION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'INCOMING_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預立勾稽代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'RELATION_CASE_SOURCE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MEMO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'REMARK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'勾稽案件來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'RELATION_CASE_IDs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EID(值機編號) + SID (音檔唯一編號)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'VOICE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'音檔相對位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'VOICE_LOCATOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源型態(來信、來電)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'SOURCE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE_SOURCE', @level2type = N'COLUMN', @level2name = N'GROUP_ID';

