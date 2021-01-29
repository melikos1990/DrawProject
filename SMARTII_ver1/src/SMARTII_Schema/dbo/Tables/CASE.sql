CREATE TABLE [dbo].[CASE] (
    [CASE_ID]                   CHAR (14)       NOT NULL,
    [NODE_ID]                   INT             NOT NULL,
    [ORGANIZATION_TYPE]         TINYINT         NOT NULL,
    [SOURCE_ID]                 CHAR (11)       NOT NULL,
    [APPLY_USERNAME]            NVARCHAR (20)   NULL,
    [APPLY_DAETTIME]            DATETIME        NULL,
    [APPLY_USER_ID]             NVARCHAR (256)  NULL,
    [CONTENT]                   NVARCHAR (MAX)  NULL,
    [CASE_TYPE]                 TINYINT         NOT NULL,
    [PROMISE_DATETIME]          DATETIME        NULL,
    [EXPECT_DATETIME]           DATETIME        NULL,
    [FILE_PATH]                 NVARCHAR (2048) NULL,
    [GROUP_ID]                  INT             CONSTRAINT [DF_CASE_GROUP_ID] DEFAULT ((0)) NULL,
    [FINISH_CONTENT]            NVARCHAR (MAX)  NULL,
    [FINISH_FILE_PATH]          NVARCHAR (2048) NOT NULL,
    [FINISH_DATETIME]           DATETIME        NULL,
    [FINISH_EML_FILE_PATH]      NVARCHAR (256)  NULL,
    [FINISH_USERNAME]           NVARCHAR (20)   NULL,
    [FINISH_REPLY_DATETIME]     DATETIME        NULL,
    [IS_REPORT]                 BIT             CONSTRAINT [DF_CASE_IS_ERROR_PROCESS] DEFAULT ((0)) NOT NULL,
    [IS_ATTENSION]              BIT             CONSTRAINT [DF_CASE_IS_ATTENSION] DEFAULT ((0)) NOT NULL,
    [QUESION_CLASSIFICATION_ID] INT             NOT NULL,
    [CASE_WARNING_ID]           INT             NOT NULL,
    [RELATION_CASE_IDs]         NVARCHAR (1024) NULL,
    [CREATE_DATETIME]           DATETIME        NOT NULL,
    [CREATE_USERNAME]           NVARCHAR (20)   NOT NULL,
    [UPDATE_DATETIME]           DATETIME        NULL,
    [UPDATE_USERNAME]           NVARCHAR (20)   NULL,
    [J_CONTENT]                 NVARCHAR (MAX)  NULL,
    [EML_FILE_PATH]             NVARCHAR (256)  NULL,
    CONSTRAINT [PK_CASE] PRIMARY KEY CLUSTERED ([CASE_ID] ASC),
    CONSTRAINT [FK_CASE_CASE_SOURCE] FOREIGN KEY ([SOURCE_ID]) REFERENCES [dbo].[CASE_SOURCE] ([SOURCE_ID]),
    CONSTRAINT [FK_CASE_CASE_WARNING] FOREIGN KEY ([CASE_WARNING_ID]) REFERENCES [dbo].[CASE_WARNING] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20200103-095556]
    ON [dbo].[CASE]([NODE_ID] ASC, [ORGANIZATION_TYPE] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K2_K13_K9_K1_K4_K23_K3_K5_K6_K7_K10_K11_8_12_14_15_16_17_18_19_20_21_22_24_25_26_27_28_29_30]
    ON [dbo].[CASE]([NODE_ID] ASC, [GROUP_ID] ASC, [CASE_TYPE] ASC, [CASE_ID] ASC, [SOURCE_ID] ASC, [CASE_WARNING_ID] ASC, [ORGANIZATION_TYPE] ASC, [APPLY_USERNAME] ASC, [APPLY_DAETTIME] ASC, [APPLY_USER_ID] ASC, [PROMISE_DATETIME] ASC, [EXPECT_DATETIME] ASC)
    INCLUDE([CONTENT], [FILE_PATH], [FINISH_CONTENT], [FINISH_FILE_PATH], [FINISH_DATETIME], [FINISH_EML_FILE_PATH], [FINISH_USERNAME], [FINISH_REPLY_DATETIME], [IS_REPORT], [IS_ATTENSION], [QUESION_CLASSIFICATION_ID], [RELATION_CASE_IDs], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_DATETIME], [UPDATE_USERNAME], [J_CONTENT], [EML_FILE_PATH]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K2_K1_K9_K13_K4_K23_K3_K5_K6_K7_K10_K11_8_12_14_15_16_17_18_19_20_21_22_24_25_26_27_28_29_30]
    ON [dbo].[CASE]([NODE_ID] ASC, [CASE_ID] ASC, [CASE_TYPE] ASC, [GROUP_ID] ASC, [SOURCE_ID] ASC, [CASE_WARNING_ID] ASC, [ORGANIZATION_TYPE] ASC, [APPLY_USERNAME] ASC, [APPLY_DAETTIME] ASC, [APPLY_USER_ID] ASC, [PROMISE_DATETIME] ASC, [EXPECT_DATETIME] ASC)
    INCLUDE([CONTENT], [FILE_PATH], [FINISH_CONTENT], [FINISH_FILE_PATH], [FINISH_DATETIME], [FINISH_EML_FILE_PATH], [FINISH_USERNAME], [FINISH_REPLY_DATETIME], [IS_REPORT], [IS_ATTENSION], [QUESION_CLASSIFICATION_ID], [RELATION_CASE_IDs], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_DATETIME], [UPDATE_USERNAME], [J_CONTENT], [EML_FILE_PATH]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K7_1_2_3_4_5_6_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30]
    ON [dbo].[CASE]([APPLY_USER_ID] ASC)
    INCLUDE([CASE_ID], [NODE_ID], [ORGANIZATION_TYPE], [SOURCE_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [CONTENT], [CASE_TYPE], [PROMISE_DATETIME], [EXPECT_DATETIME], [FILE_PATH], [GROUP_ID], [FINISH_CONTENT], [FINISH_FILE_PATH], [FINISH_DATETIME], [FINISH_EML_FILE_PATH], [FINISH_USERNAME], [FINISH_REPLY_DATETIME], [IS_REPORT], [IS_ATTENSION], [QUESION_CLASSIFICATION_ID], [CASE_WARNING_ID], [RELATION_CASE_IDs], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_DATETIME], [UPDATE_USERNAME], [J_CONTENT], [EML_FILE_PATH]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K1_2_9_13_21]
    ON [dbo].[CASE]([CASE_ID] ASC)
    INCLUDE([NODE_ID], [CASE_TYPE], [GROUP_ID], [IS_ATTENSION]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K2_K21_K13_K9_K1]
    ON [dbo].[CASE]([NODE_ID] ASC, [IS_ATTENSION] ASC, [GROUP_ID] ASC, [CASE_TYPE] ASC, [CASE_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K1_K2_K3_K4_K5_K6_K7_K9_K10_K11_8_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30]
    ON [dbo].[CASE]([CASE_ID] ASC, [NODE_ID] ASC, [ORGANIZATION_TYPE] ASC, [SOURCE_ID] ASC, [APPLY_USERNAME] ASC, [APPLY_DAETTIME] ASC, [APPLY_USER_ID] ASC, [CASE_TYPE] ASC, [PROMISE_DATETIME] ASC, [EXPECT_DATETIME] ASC)
    INCLUDE([CONTENT], [FILE_PATH], [GROUP_ID], [FINISH_CONTENT], [FINISH_FILE_PATH], [FINISH_DATETIME], [FINISH_EML_FILE_PATH], [FINISH_USERNAME], [FINISH_REPLY_DATETIME], [IS_REPORT], [IS_ATTENSION], [QUESION_CLASSIFICATION_ID], [CASE_WARNING_ID], [RELATION_CASE_IDs], [CREATE_DATETIME], [CREATE_USERNAME], [UPDATE_DATETIME], [UPDATE_USERNAME], [J_CONTENT], [EML_FILE_PATH]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_23_282484085__K1_K22]
    ON [dbo].[CASE]([CASE_ID] ASC, [QUESION_CLASSIFICATION_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_12_282484085__K1_4364]
    ON [dbo].[CASE]([CASE_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CASE_12_282484085__K1_K22]
    ON [dbo].[CASE]([CASE_ID] ASC, [QUESION_CLASSIFICATION_ID] ASC);


GO
CREATE STATISTICS [_dta_stat_282484085_2_9]
    ON [dbo].[CASE]([NODE_ID], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_21_1]
    ON [dbo].[CASE]([IS_ATTENSION], [CASE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_3_1]
    ON [dbo].[CASE]([ORGANIZATION_TYPE], [CASE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_21_13]
    ON [dbo].[CASE]([IS_ATTENSION], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_21_13]
    ON [dbo].[CASE]([CASE_ID], [IS_ATTENSION], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_23_13_2]
    ON [dbo].[CASE]([CASE_WARNING_ID], [GROUP_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_4_13_2]
    ON [dbo].[CASE]([SOURCE_ID], [GROUP_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_13_9]
    ON [dbo].[CASE]([CASE_ID], [GROUP_ID], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_23_4_9]
    ON [dbo].[CASE]([CASE_ID], [CASE_WARNING_ID], [SOURCE_ID], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_23_13_9_21]
    ON [dbo].[CASE]([CASE_WARNING_ID], [GROUP_ID], [CASE_TYPE], [IS_ATTENSION]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_23_13_9]
    ON [dbo].[CASE]([CASE_ID], [CASE_WARNING_ID], [GROUP_ID], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_13_2_9_23]
    ON [dbo].[CASE]([GROUP_ID], [NODE_ID], [CASE_TYPE], [CASE_WARNING_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_21_13]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [IS_ATTENSION], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_13_2_9_4]
    ON [dbo].[CASE]([GROUP_ID], [NODE_ID], [CASE_TYPE], [SOURCE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_4_13_9_21]
    ON [dbo].[CASE]([SOURCE_ID], [GROUP_ID], [CASE_TYPE], [IS_ATTENSION]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_23_9_13_2]
    ON [dbo].[CASE]([CASE_ID], [CASE_WARNING_ID], [CASE_TYPE], [GROUP_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_21_13]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [IS_ATTENSION], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_4_1_9_13_2]
    ON [dbo].[CASE]([SOURCE_ID], [CASE_ID], [CASE_TYPE], [GROUP_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_2_13]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [NODE_ID], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_21_9]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [IS_ATTENSION], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_2_9]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [NODE_ID], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_2_21_13]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [NODE_ID], [IS_ATTENSION], [GROUP_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_13_9_21_1_23_2]
    ON [dbo].[CASE]([GROUP_ID], [CASE_TYPE], [IS_ATTENSION], [CASE_ID], [CASE_WARNING_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_2_21_9]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [NODE_ID], [IS_ATTENSION], [CASE_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_13_9_4_23]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [GROUP_ID], [CASE_TYPE], [SOURCE_ID], [CASE_WARNING_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_3_21_13_9_4]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [ORGANIZATION_TYPE], [IS_ATTENSION], [GROUP_ID], [CASE_TYPE], [SOURCE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_13_2_23_4_3_5_6_7]
    ON [dbo].[CASE]([CASE_ID], [GROUP_ID], [NODE_ID], [CASE_WARNING_ID], [SOURCE_ID], [ORGANIZATION_TYPE], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_3_4_5_6_7_9_10_11]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [ORGANIZATION_TYPE], [SOURCE_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [CASE_TYPE], [PROMISE_DATETIME], [EXPECT_DATETIME]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_23_4_5_6_7_9_10_11_2]
    ON [dbo].[CASE]([CASE_ID], [CASE_WARNING_ID], [SOURCE_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [CASE_TYPE], [PROMISE_DATETIME], [EXPECT_DATETIME], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_3_13_9_4_23_5_6_7_10]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [ORGANIZATION_TYPE], [GROUP_ID], [CASE_TYPE], [SOURCE_ID], [CASE_WARNING_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [PROMISE_DATETIME]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_23_2_3_4_5_6_7_9_10_11]
    ON [dbo].[CASE]([CASE_ID], [CASE_WARNING_ID], [NODE_ID], [ORGANIZATION_TYPE], [SOURCE_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [CASE_TYPE], [PROMISE_DATETIME], [EXPECT_DATETIME]);


GO
CREATE STATISTICS [_dta_stat_282484085_4_1_13_9_21_23_5_6_7_10_11_2]
    ON [dbo].[CASE]([SOURCE_ID], [CASE_ID], [GROUP_ID], [CASE_TYPE], [IS_ATTENSION], [CASE_WARNING_ID], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [PROMISE_DATETIME], [EXPECT_DATETIME], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_4_23_13_9_5_6_7_10_11_2_3]
    ON [dbo].[CASE]([CASE_ID], [SOURCE_ID], [CASE_WARNING_ID], [GROUP_ID], [CASE_TYPE], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [PROMISE_DATETIME], [EXPECT_DATETIME], [NODE_ID], [ORGANIZATION_TYPE]);


GO
CREATE STATISTICS [_dta_stat_282484085_9_1_2_21_13_4_23_3_5_6_7_10_11]
    ON [dbo].[CASE]([CASE_TYPE], [CASE_ID], [NODE_ID], [IS_ATTENSION], [GROUP_ID], [SOURCE_ID], [CASE_WARNING_ID], [ORGANIZATION_TYPE], [APPLY_USERNAME], [APPLY_DAETTIME], [APPLY_USER_ID], [PROMISE_DATETIME], [EXPECT_DATETIME]);


GO
CREATE STATISTICS [_dta_stat_282484085_22_1]
    ON [dbo].[CASE]([QUESION_CLASSIFICATION_ID], [CASE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_2_13_22]
    ON [dbo].[CASE]([NODE_ID], [GROUP_ID], [QUESION_CLASSIFICATION_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_2_13_25]
    ON [dbo].[CASE]([NODE_ID], [GROUP_ID], [CREATE_DATETIME]);


GO
CREATE STATISTICS [_dta_stat_282484085_22_13_2]
    ON [dbo].[CASE]([QUESION_CLASSIFICATION_ID], [GROUP_ID], [NODE_ID]);


GO
CREATE STATISTICS [_dta_stat_282484085_1_2_13_22]
    ON [dbo].[CASE]([CASE_ID], [NODE_ID], [GROUP_ID], [QUESION_CLASSIFICATION_ID]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件編號 (BU代號3碼 + yyMMdd + 流水號5碼)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CASE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號 (企業別)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'NODE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'ORGANIZATION_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'SOURCE_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'APPLY_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人建立時間(同立案時間/案件移轉認養時間)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'APPLY_DAETTIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件負責人代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'APPLY_USER_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'反應內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 : 立案 ; 1 : 處理中 ; 2 : 結案', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CASE_TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'應完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'PROMISE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶期望完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'EXPECT_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'案件附件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'GROUP_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FINISH_CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案附件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FINISH_FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FINISH_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案EMAIL儲存路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FINISH_EML_FILE_PATH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結案回覆時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'FINISH_REPLY_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否列入日報', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'IS_REPORT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否待關注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'IS_ATTENSION';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題分類代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'QUESION_CLASSIFICATION_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'緊急程度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CASE_WARNING_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'相關案件代號清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'RELATION_CASE_IDs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CREATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'CREATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'UPDATE_DATETIME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'UPDATE_USERNAME';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'其他資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'J_CONTENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'認養工單的信件路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CASE', @level2type = N'COLUMN', @level2name = N'EML_FILE_PATH';

