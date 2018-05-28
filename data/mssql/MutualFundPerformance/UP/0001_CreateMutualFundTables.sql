CREATE SCHEMA MutualFund
GO

CREATE TABLE [MutualFund].[MutualFund](
	[MutualFundId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL
 CONSTRAINT [PK_MutualFund_MutualFund] PRIMARY KEY CLUSTERED 
(
	[MutualFundId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [MutualFund].[Benchmark](
	[BenchmarkId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[MutualFundId] [uniqueidentifier] NOT NULL,
	[SortOrder] [int] not null,
 CONSTRAINT [PK_MutualFund_Benchmark] PRIMARY KEY CLUSTERED 
(
	[BenchmarkId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [MutualFund].[Benchmark]  WITH CHECK ADD  CONSTRAINT [FK_MutualFund_Benchmark_MutualFundId] FOREIGN KEY([MutualFundId])
REFERENCES [MutualFund].[MutualFund] ([MutualFundId])
GO

