USE OPTrade
GO

--������ PartitionCode CHAR(1) => CHAR(4)
--ALTER TABLE ReportTradeActivityDetail ALTER COLUMN [PartitionCode] [CHAR](4);

/****** Object:  Table [dbo].[ReportTradeActivityDetail_New]    Script Date: 2021/2/3 �W�� 10:19:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ReportTradeActivityDetail_New](
	[TradeTime] [datetime2](7) NOT NULL,
	[ActivityItemCode] [char](10) NULL,
	[ActivityName] [nvarchar](30) NULL,
	[LargeCategoryName] [nvarchar](20) NULL,
	[SmallCategoryName] [nvarchar](20) NULL,
	[ActivityID] [char](10) NULL,
	[VoucherHeaderCode] [char](16) NULL,
	[ItemName] [nvarchar](30) NULL,
	[ItemCode] [char](40) NULL,
	[DisplayCode] [nvarchar](30) NULL,
	[BrandName] [nvarchar](50) NULL,
	[VoucherType] [int] NULL,
	[PerExchangePoint] [int] NULL,
	[ExchangeQty] [int] NULL,
	[mid] [char](32) NULL,
	[TransactionID] [char](57) NOT NULL,
	[PartitionCode] [char](4) NOT NULL,
	[VoucherHeaderItemCode] [char](6) NULL,
	[ActivityItemName] [nvarchar](30) NULL,
 CONSTRAINT [PK_ForReport_New] PRIMARY KEY CLUSTERED 
(
	[TradeTime] ASC,
	[TransactionID] ASC,
	[PartitionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'TradeTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʶ���Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityItemCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���饻�������ʦW��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�j�����W��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'LargeCategoryName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�p�����W��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'SmallCategoryName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���饻��������id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����header code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'VoucherHeaderCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�~���W��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ItemName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���饻����item code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ItemCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�~��-set�D��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'DisplayCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�q���W��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'BrandName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'VoucherType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�I�����(�I)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'PerExchangePoint'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�I���ƶq' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ExchangeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�|��mid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'mid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'TransactionID'
GO


