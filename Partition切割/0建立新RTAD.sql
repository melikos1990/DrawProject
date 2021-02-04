USE OPTrade
GO

--更改欄位 PartitionCode CHAR(1) => CHAR(4)
--ALTER TABLE ReportTradeActivityDetail ALTER COLUMN [PartitionCode] [CHAR](4);

/****** Object:  Table [dbo].[ReportTradeActivityDetail_New]    Script Date: 2021/2/3 上午 10:19:05 ******/
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'TradeTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活動項目Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityItemCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票券本身的活動名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大分類名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'LargeCategoryName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小分類名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'SmallCategoryName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票券本身的活動id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ActivityID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票券header code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'VoucherHeaderCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品項名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ItemName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票券本身的item code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ItemCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品號-set主檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'DisplayCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通路名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'BrandName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票券類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'VoucherType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'兌換單價(點)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'PerExchangePoint'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'兌換數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'ExchangeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'會員mid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'mid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportTradeActivityDetail_New', @level2type=N'COLUMN',@level2name=N'TransactionID'
GO


