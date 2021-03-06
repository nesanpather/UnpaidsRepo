USE [master]
GO

/****** Object:  Login [UnpaidsUser]    Script Date: 2019/01/18 16:34:41 ******/
CREATE LOGIN [UnpaidsUser] WITH PASSWORD=N'Password1234$', DEFAULT_DATABASE=[Unpaids], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [Unpaids]
GO
/****** Object:  User [UnpaidsUser]    Script Date: 2019/01/18 16:34:41 ******/
CREATE USER [UnpaidsUser] FOR LOGIN [UnpaidsUser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [UnpaidsUser]
GO
/****** Object:  Table [dbo].[tb_AccessToken]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_AccessToken](
	[AccessTokenId] [int] IDENTITY(1,1) NOT NULL,
	[AccessToken] [nvarchar](1000) NOT NULL,
	[TokenType] [nvarchar](20) NOT NULL,
	[ExpiresIn] [int] NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[DateExpires] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_AccessToken] PRIMARY KEY CLUSTERED 
(
	[AccessTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Notification]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Notification](
	[NotificationId] [int] NOT NULL,
	[Notification] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tb_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Response]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Response](
	[ResponseId] [int] NOT NULL,
	[Response] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tb_Response] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Status]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Status](
	[StatusId] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tb_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Unpaid]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Unpaid](
	[UnpaidId] [int] IDENTITY(1,1) NOT NULL,
	[PolicyNumber] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Message] [nvarchar](200) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[IdNumber] [nvarchar](50) NOT NULL,
	[UnpaidBatchId] [int] NOT NULL,
 CONSTRAINT [PK_tb_Unpaid] PRIMARY KEY CLUSTERED 
(
	[UnpaidId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_UnpaidBatch]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_UnpaidBatch](
	[UnpaidBatchId] [int] IDENTITY(1,1) NOT NULL,
	[BatchKey] [nvarchar](200) NOT NULL,
	[StatusId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[DateModified] [datetime] NULL,
 CONSTRAINT [PK_tb_UnpaidBatch] PRIMARY KEY CLUSTERED 
(
	[UnpaidBatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_UnpaidRequest]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_UnpaidRequest](
	[UnpaidRequestId] [int] IDENTITY(1,1) NOT NULL,
	[UnpaidId] [int] NOT NULL,
	[NotificationId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[StatusAdditionalInfo] [nvarchar](200) NULL,
	[DateModified] [datetime] NULL,
	[CorrelationId] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_tb_UnpaidRequest] PRIMARY KEY CLUSTERED 
(
	[UnpaidRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_UnpaidResponse]    Script Date: 2019/01/23 14:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_UnpaidResponse](
	[UnpaidResponseId] [int] IDENTITY(1,1) NOT NULL,
	[UnpaidRequestId] [int] NOT NULL,
	[ResponseId] [int] NOT NULL,
	[Accepted] [bit] NOT NULL,
	[StatusId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_UnpaidResponse] PRIMARY KEY CLUSTERED 
(
	[UnpaidResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_Unpaid] ADD  CONSTRAINT [DF_tb_Unpaid_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[tb_UnpaidBatch] ADD  CONSTRAINT [DF_tb_UnpaidBatch_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[tb_UnpaidBatch] ADD  CONSTRAINT [DF_tb_UnpaidBatch_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[tb_UnpaidRequest] ADD  CONSTRAINT [DF_tb_UnpaidRequest_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[tb_UnpaidRequest] ADD  CONSTRAINT [DF_tb_UnpaidRequest_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[tb_UnpaidResponse] ADD  CONSTRAINT [DF_tb_UnpaidResponse_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[tb_Unpaid]  WITH CHECK ADD  CONSTRAINT [FK_tb_Unpaid_tb_UnpaidBatch] FOREIGN KEY([UnpaidBatchId])
REFERENCES [dbo].[tb_UnpaidBatch] ([UnpaidBatchId])
GO
ALTER TABLE [dbo].[tb_Unpaid] CHECK CONSTRAINT [FK_tb_Unpaid_tb_UnpaidBatch]
GO
ALTER TABLE [dbo].[tb_UnpaidBatch]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidBatch_tb_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tb_Status] ([StatusId])
GO
ALTER TABLE [dbo].[tb_UnpaidBatch] CHECK CONSTRAINT [FK_tb_UnpaidBatch_tb_Status]
GO
ALTER TABLE [dbo].[tb_UnpaidRequest]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidRequest_tb_Notification] FOREIGN KEY([NotificationId])
REFERENCES [dbo].[tb_Notification] ([NotificationId])
GO
ALTER TABLE [dbo].[tb_UnpaidRequest] CHECK CONSTRAINT [FK_tb_UnpaidRequest_tb_Notification]
GO
ALTER TABLE [dbo].[tb_UnpaidRequest]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidRequest_tb_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tb_Status] ([StatusId])
GO
ALTER TABLE [dbo].[tb_UnpaidRequest] CHECK CONSTRAINT [FK_tb_UnpaidRequest_tb_Status]
GO
ALTER TABLE [dbo].[tb_UnpaidRequest]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidRequest_tb_Unpaid] FOREIGN KEY([UnpaidId])
REFERENCES [dbo].[tb_Unpaid] ([UnpaidId])
GO
ALTER TABLE [dbo].[tb_UnpaidRequest] CHECK CONSTRAINT [FK_tb_UnpaidRequest_tb_Unpaid]
GO
ALTER TABLE [dbo].[tb_UnpaidResponse]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidResponse_tb_Response] FOREIGN KEY([ResponseId])
REFERENCES [dbo].[tb_Response] ([ResponseId])
GO
ALTER TABLE [dbo].[tb_UnpaidResponse] CHECK CONSTRAINT [FK_tb_UnpaidResponse_tb_Response]
GO
ALTER TABLE [dbo].[tb_UnpaidResponse]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidResponse_tb_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tb_Status] ([StatusId])
GO
ALTER TABLE [dbo].[tb_UnpaidResponse] CHECK CONSTRAINT [FK_tb_UnpaidResponse_tb_Status]
GO
ALTER TABLE [dbo].[tb_UnpaidResponse]  WITH CHECK ADD  CONSTRAINT [FK_tb_UnpaidResponse_tb_UnpaidRequest] FOREIGN KEY([UnpaidRequestId])
REFERENCES [dbo].[tb_UnpaidRequest] ([UnpaidRequestId])
GO
ALTER TABLE [dbo].[tb_UnpaidResponse] CHECK CONSTRAINT [FK_tb_UnpaidResponse_tb_UnpaidRequest]
GO