CREATE SCHEMA StockPrice
GO

CREATE TABLE [StockPrice].[InvestmentVehicle](
	[InvestmentVehicleId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ExternalId] [uniqueidentifier] NOT NULL
 CONSTRAINT [PK_StockPrice_InvestmentVehicle_InvestmentVehicleId] PRIMARY KEY CLUSTERED 
(
	[InvestmentVehicleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [StockPrice].[Price](
	[InvestmentVehicleId] [uniqueidentifier] NOT NULL,
	[CloseDate] [DateTime],
	[Price] decimal(16, 8) NOT NULL
 CONSTRAINT [PK_StockPrice_Price_InvestmentVehicleId_CloseDate] PRIMARY KEY CLUSTERED 
(
	[InvestmentVehicleId] ASC,
	[CloseDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [StockPrice].[Price]  WITH CHECK ADD  CONSTRAINT [FK_StockPrice_Price_InvestmentVehicleId] FOREIGN KEY([InvestmentVehicleId])
REFERENCES [StockPrice].[InvestmentVehicle] ([InvestmentVehicleId])
GO
