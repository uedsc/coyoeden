/****** CoyoEden.NET 1.5 SQL Setup Script ******/

/****** Object:  Table [dbo].[cy_Categories]    Script Date: 12/22/2007 14:14:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Categories](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Categories_Id]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[ParentID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_cy_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_Pages]    Script Date: 12/22/2007 14:15:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Pages](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Pages_Id]  DEFAULT (newid()),
	[Title] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[PageContent] [nvarchar](max) NULL,
	[Keywords] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[IsPublished] [bit] NULL,
	[IsFrontPage] [bit] NULL,
	[Parent] [uniqueidentifier] NULL,
	[ShowInList] [bit] NULL,
 CONSTRAINT [PK_cy_Pages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_PingService]    Script Date: 12/22/2007 14:15:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_PingService](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_PingService_Id]  DEFAULT (newid()),
	[Link] [nvarchar](255) NULL,
 CONSTRAINT [PK_cy_PingService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_Posts]    Script Date: 12/22/2007 14:16:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Posts](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Posts_Id]  DEFAULT (newid()),
	[Title] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[PostContent] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[IsPublished] [bit] NULL,
	[IsCommentEnabled] [bit] NULL,
	[Raters] [int] NULL,
	[Rating] [real] NULL,
	[Slug] [nvarchar](255) NULL,
 CONSTRAINT [PK_cy_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_Settings]    Script Date: 12/22/2007 14:16:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Settings](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[SettingType] [nvarchar](50) NOT NULL,
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_cy_Settings_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cy_Settings] ADD  CONSTRAINT [DF_cy_Settings_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[cy_Settings] ADD  CONSTRAINT [DF_cy_Settings_SettingType]  DEFAULT (N'AppSetting') FOR [SettingType]
GO
/****** Object:  Table [dbo].[cy_Profiles]    Script Date: 06/28/2008 19:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Profiles](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Profiles_Id]  DEFAULT (newid()),
	[UserName] [nvarchar](100) NULL,
	[SettingName] [nvarchar](200) NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_cy_Profiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_StopWords]    Script Date: 06/28/2008 19:33:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_StopWords](
	[Word] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_cy_StopWords] PRIMARY KEY CLUSTERED 
(
	[Word] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_PostCategory]    Script Date: 12/22/2007 14:17:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_PostCategory](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_PostCategory_Id]  DEFAULT (newid()),
	[PostID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_cy_PostCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cy_PostCategory]  WITH CHECK ADD  CONSTRAINT [FK_cy_PostCategory_cy_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[cy_Categories] ([Id])
GO
ALTER TABLE [dbo].[cy_PostCategory] CHECK CONSTRAINT [FK_cy_PostCategory_cy_Categories]
GO
ALTER TABLE [dbo].[cy_PostCategory]  WITH CHECK ADD  CONSTRAINT [FK_cy_PostCategory_cy_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[cy_Posts] ([Id])
GO
ALTER TABLE [dbo].[cy_PostCategory] CHECK CONSTRAINT [FK_cy_PostCategory_cy_Posts]
GO
/****** Object:  Table [dbo].[cy_PostComment]    Script Date: 12/22/2007 14:17:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_PostComment](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_PostComment_Id]  DEFAULT (newid()),
	[PostID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NOT NULL DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[DateCreated] [datetime] NOT NULL,
	[Author] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Website] [nvarchar](255) NULL,
	[Comment] [nvarchar](max) NULL,
	[Country] [nvarchar](255) NULL,
	[Ip] [nvarchar](50) NULL,
	[IsApproved] [bit] NULL,
 CONSTRAINT [PK_cy_PostComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cy_PostComment]  WITH CHECK ADD  CONSTRAINT [FK_cy_PostComment_cy_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[cy_Posts] ([Id])
GO
ALTER TABLE [dbo].[cy_PostComment] CHECK CONSTRAINT [FK_cy_PostComment_cy_Posts]
GO
/****** Object:  Table [dbo].[cy_PostNotify]    Script Date: 12/22/2007 14:17:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_PostNotify](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_PostNotify_Id]  DEFAULT (newid()),
	[PostID] [uniqueidentifier] NOT NULL,
	[NotifyAddress] [nvarchar](255) NULL,
 CONSTRAINT [PK_cy_PostNotify] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cy_PostNotify]  WITH CHECK ADD  CONSTRAINT [FK_cy_PostNotify_cy_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[cy_Posts] ([Id])
GO
ALTER TABLE [dbo].[cy_PostNotify] CHECK CONSTRAINT [FK_cy_PostNotify_cy_Posts]
GO
/****** Object:  Table [dbo].[cy_PostTag]    Script Date: 12/22/2007 14:17:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_PostTag](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_PostTag_Id]  DEFAULT (newid()),
	[PostID] [uniqueidentifier] NOT NULL,
	[Tag] [nvarchar](50) NULL,
 CONSTRAINT [PK_cy_PostTag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cy_PostTag]  WITH CHECK ADD  CONSTRAINT [FK_cy_PostTag_cy_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[cy_Posts] ([Id])
GO
ALTER TABLE [dbo].[cy_PostTag] CHECK CONSTRAINT [FK_cy_PostTag_cy_Posts]
GO
/****** Object:  Table [dbo].[cy_Users]    Script Date: 07/30/2008 21:55:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Users](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Users_Id]  DEFAULT (newid()),
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[EmailAddress] [nvarchar](100) NULL,
 CONSTRAINT [PK_cy_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_Roles]    Script Date: 07/30/2008 21:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_Roles](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_Roles_Id]  DEFAULT (newid()),
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_cy_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cy_UserRoles]    Script Date: 07/31/2008 12:26:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cy_UserRoles](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_cy_UserRoles_Id]  DEFAULT (newid()),
	[UserID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_cy_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cy_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_cy_UserRoles_cy_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[cy_Roles] ([Id])
GO
ALTER TABLE [dbo].[cy_UserRoles] CHECK CONSTRAINT [FK_cy_UserRoles_cy_Roles]
GO
ALTER TABLE [dbo].[cy_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_cy_UserRoles_cy_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[cy_Users] ([Id])
GO
ALTER TABLE [dbo].[cy_UserRoles] CHECK CONSTRAINT [FK_cy_UserRoles_cy_Users]
GO
/****** Object:  Index [FK_PostID]    Script Date: 12/22/2007 14:18:36 ******/
CREATE NONCLUSTERED INDEX [FK_PostID] ON [dbo].[cy_PostCategory] 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [FK_CategoryID]    Script Date: 12/22/2007 14:19:19 ******/
CREATE NONCLUSTERED INDEX [FK_CategoryID] ON [dbo].[cy_PostCategory] 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [FK_PostID]    Script Date: 12/22/2007 14:19:45 ******/
CREATE NONCLUSTERED INDEX [FK_PostID] ON [dbo].[cy_PostComment] 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [FK_PostID]    Script Date: 12/22/2007 14:20:29 ******/
CREATE NONCLUSTERED INDEX [FK_PostID] ON [dbo].[cy_PostNotify] 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [FK_PostID]    Script Date: 12/22/2007 14:20:43 ******/
CREATE NONCLUSTERED INDEX [FK_PostID] ON [dbo].[cy_PostTag] 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [I_TypeID]    Script Date: 06/28/2008 19:34:43 ******/
CREATE NONCLUSTERED INDEX [I_TypeID] ON [dbo].[cy_DataStoreSettings] 
(
	[ExtensionType] ASC,
	[ExtensionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Index [I_UserName]    Script Date: 06/28/2008 19:35:12 ******/
CREATE NONCLUSTERED INDEX [I_UserName] ON [dbo].[cy_Profiles] 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/***  Load initial Data ***/
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('administratorrole', 'Administrators');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('alternatefeedurl', '');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('authorname', 'My name');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('avatar', 'combine');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('blogrollmaxlength', '23');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('blogrollupdateminutes', '60');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('blogrollvisibleposts', '3');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('contactformmessage', '<p>I will answer the mail as soon as I can.</p>');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('contactthankmessage', '<h1>Thank you</h1><p>The message was sent.</p>');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('culture', 'Auto');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('dayscommentsareenabled', '0');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('description', 'Short description of the blog');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('displaycommentsonrecentposts', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('displayratingsonrecentposts', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('email', 'user@example.com');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('emailsubjectprefix', 'Weblog');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablecommentsearch', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablecommentsmoderation', 'False');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablecontactattachments', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablecountryincomments', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablehttpcompression', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enableopensearch', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablepingbackreceive', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablepingbacksend', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablerating', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablereferrertracking', 'False');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablerelatedposts', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enablessl', 'False');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enabletrackbackreceive', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('enabletrackbacksend', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('endorsement', 'http://www.dotnetCoyoEden.net/syndication.axd');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('fileextension', '.aspx');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('geocodinglatitude', '0');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('geocodinglongitude', '0');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('handlewwwsubdomain', '');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('iscocommentenabled', 'False');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('IsCommentEnabled', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('language', 'en-GB');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('mobiletheme', 'Mobile');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('name', 'Name of the blog');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('numberofrecentcomments', '10');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('numberofrecentposts', '10');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('postsperfeed', '10');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('postsperpage', '10');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('removewhitespaceinstylesheets', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('searchbuttontext', 'Search');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('searchcommentlabeltext', 'Include comments in search');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('searchdefaulttext', 'Enter search term');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('sendmailoncomment', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('showdescriptioninpostlist', 'False');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('showlivepreview', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('showpostnavigation', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('smtppassword', 'password');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('smtpserver', 'mail.example.dk');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('smtpserverport', '25');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('smtpusername', 'user@example.com');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('storagelocation', '~/App_Data/');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('syndicationformat', 'Rss');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('theme', 'Standard');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('timestamppostlinks', 'True');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('timezone', '-5');
INSERT INTO cy_Settings (SettingName, SettingValue)	VALUES ('trackingscript', '');
INSERT INTO cy_Settings (SettingType,SettingName,[SettingValue]) 
VALUES ('ExtensionSetting','cy_WidgetZone0','<?xml version="1.0" encoding="utf-16"?>
<WidgetData xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Settings>&lt;widgets&gt;&lt;widget id="d9ada63d-3462-4c72-908e-9d35f0acce40" title="TextBox" showTitle="True"&gt;TextBox&lt;/widget&gt;&lt;widget id="19baa5f6-49d4-4828-8f7f-018535c35f94" title="Administration" showTitle="True"&gt;Administration&lt;/widget&gt;&lt;widget id="d81c5ae3-e57e-4374-a539-5cdee45e639f" title="Search" showTitle="True"&gt;Search&lt;/widget&gt;&lt;widget id="77142800-6dff-4016-99ca-69b5c5ebac93" title="Tag cloud" showTitle="True"&gt;Tag cloud&lt;/widget&gt;&lt;widget id="4ce68ae7-c0c8-4bf8-b50f-a67b582b0d2e" title="RecentPosts" showTitle="True"&gt;RecentPosts&lt;/widget&gt;&lt;/widgets&gt;</Settings>
</WidgetData>');

INSERT INTO cy_PingService (Link) VALUES ('http://rpc.technorati.com/rpc/ping');
INSERT INTO cy_PingService (Link) VALUES ('http://rpc.pingomatic.com/rpc2');
INSERT INTO cy_PingService (Link) VALUES ('http://ping.feedburner.com');
INSERT INTO cy_PingService (Link) VALUES ('http://www.bloglines.com/ping');
INSERT INTO cy_PingService (Link) VALUES ('http://services.newsgator.com/ngws/xmlrpcping.aspx');
INSERT INTO cy_PingService (Link) VALUES ('http://api.my.yahoo.com/rpc2 ');
INSERT INTO cy_PingService (Link) VALUES ('http://blogsearch.google.com/ping/RPC2');
INSERT INTO cy_PingService (Link) VALUES ('http://rpc.pingthesemanticweb.com/');

INSERT INTO cy_StopWords (Word)	VALUES ('a');
INSERT INTO cy_StopWords (Word)	VALUES ('about');
INSERT INTO cy_StopWords (Word)	VALUES ('actually');
INSERT INTO cy_StopWords (Word)	VALUES ('add');
INSERT INTO cy_StopWords (Word)	VALUES ('after');
INSERT INTO cy_StopWords (Word)	VALUES ('all');
INSERT INTO cy_StopWords (Word)	VALUES ('almost');
INSERT INTO cy_StopWords (Word)	VALUES ('along');
INSERT INTO cy_StopWords (Word)	VALUES ('also');
INSERT INTO cy_StopWords (Word)	VALUES ('an');
INSERT INTO cy_StopWords (Word)	VALUES ('and');
INSERT INTO cy_StopWords (Word)	VALUES ('any');
INSERT INTO cy_StopWords (Word)	VALUES ('are');
INSERT INTO cy_StopWords (Word)	VALUES ('as');
INSERT INTO cy_StopWords (Word)	VALUES ('at');
INSERT INTO cy_StopWords (Word)	VALUES ('be');
INSERT INTO cy_StopWords (Word)	VALUES ('both');
INSERT INTO cy_StopWords (Word)	VALUES ('but');
INSERT INTO cy_StopWords (Word)	VALUES ('by');
INSERT INTO cy_StopWords (Word)	VALUES ('can');
INSERT INTO cy_StopWords (Word)	VALUES ('cannot');
INSERT INTO cy_StopWords (Word)	VALUES ('com');
INSERT INTO cy_StopWords (Word)	VALUES ('could');
INSERT INTO cy_StopWords (Word)	VALUES ('de');
INSERT INTO cy_StopWords (Word)	VALUES ('do');
INSERT INTO cy_StopWords (Word)	VALUES ('down');
INSERT INTO cy_StopWords (Word)	VALUES ('each');
INSERT INTO cy_StopWords (Word)	VALUES ('either');
INSERT INTO cy_StopWords (Word)	VALUES ('en');
INSERT INTO cy_StopWords (Word)	VALUES ('for');
INSERT INTO cy_StopWords (Word)	VALUES ('from');
INSERT INTO cy_StopWords (Word)	VALUES ('good');
INSERT INTO cy_StopWords (Word)	VALUES ('has');
INSERT INTO cy_StopWords (Word)	VALUES ('have');
INSERT INTO cy_StopWords (Word)	VALUES ('he');
INSERT INTO cy_StopWords (Word)	VALUES ('her');
INSERT INTO cy_StopWords (Word)	VALUES ('here');
INSERT INTO cy_StopWords (Word)	VALUES ('hers');
INSERT INTO cy_StopWords (Word)	VALUES ('his');
INSERT INTO cy_StopWords (Word)	VALUES ('how');
INSERT INTO cy_StopWords (Word)	VALUES ('i');
INSERT INTO cy_StopWords (Word)	VALUES ('if');
INSERT INTO cy_StopWords (Word)	VALUES ('in');
INSERT INTO cy_StopWords (Word)	VALUES ('into');
INSERT INTO cy_StopWords (Word)	VALUES ('is');
INSERT INTO cy_StopWords (Word)	VALUES ('it');
INSERT INTO cy_StopWords (Word)	VALUES ('its');
INSERT INTO cy_StopWords (Word)	VALUES ('just');
INSERT INTO cy_StopWords (Word)	VALUES ('la');
INSERT INTO cy_StopWords (Word)	VALUES ('like');
INSERT INTO cy_StopWords (Word)	VALUES ('long');
INSERT INTO cy_StopWords (Word)	VALUES ('make');
INSERT INTO cy_StopWords (Word)	VALUES ('me');
INSERT INTO cy_StopWords (Word)	VALUES ('more');
INSERT INTO cy_StopWords (Word)	VALUES ('much');
INSERT INTO cy_StopWords (Word)	VALUES ('my');
INSERT INTO cy_StopWords (Word)	VALUES ('need');
INSERT INTO cy_StopWords (Word)	VALUES ('new');
INSERT INTO cy_StopWords (Word)	VALUES ('now');
INSERT INTO cy_StopWords (Word)	VALUES ('of');
INSERT INTO cy_StopWords (Word)	VALUES ('off');
INSERT INTO cy_StopWords (Word)	VALUES ('on');
INSERT INTO cy_StopWords (Word)	VALUES ('once');
INSERT INTO cy_StopWords (Word)	VALUES ('one');
INSERT INTO cy_StopWords (Word)	VALUES ('ones');
INSERT INTO cy_StopWords (Word)	VALUES ('only');
INSERT INTO cy_StopWords (Word)	VALUES ('or');
INSERT INTO cy_StopWords (Word)	VALUES ('our');
INSERT INTO cy_StopWords (Word)	VALUES ('out');
INSERT INTO cy_StopWords (Word)	VALUES ('over');
INSERT INTO cy_StopWords (Word)	VALUES ('own');
INSERT INTO cy_StopWords (Word)	VALUES ('really');
INSERT INTO cy_StopWords (Word)	VALUES ('right');
INSERT INTO cy_StopWords (Word)	VALUES ('same');
INSERT INTO cy_StopWords (Word)	VALUES ('see');
INSERT INTO cy_StopWords (Word)	VALUES ('she');
INSERT INTO cy_StopWords (Word)	VALUES ('so');
INSERT INTO cy_StopWords (Word)	VALUES ('some');
INSERT INTO cy_StopWords (Word)	VALUES ('such');
INSERT INTO cy_StopWords (Word)	VALUES ('take');
INSERT INTO cy_StopWords (Word)	VALUES ('takes');
INSERT INTO cy_StopWords (Word)	VALUES ('that');
INSERT INTO cy_StopWords (Word)	VALUES ('the');
INSERT INTO cy_StopWords (Word)	VALUES ('their');
INSERT INTO cy_StopWords (Word)	VALUES ('these');
INSERT INTO cy_StopWords (Word)	VALUES ('thing');
INSERT INTO cy_StopWords (Word)	VALUES ('this');
INSERT INTO cy_StopWords (Word)	VALUES ('to');
INSERT INTO cy_StopWords (Word)	VALUES ('too');
INSERT INTO cy_StopWords (Word)	VALUES ('took');
INSERT INTO cy_StopWords (Word)	VALUES ('und');
INSERT INTO cy_StopWords (Word)	VALUES ('up');
INSERT INTO cy_StopWords (Word)	VALUES ('use');
INSERT INTO cy_StopWords (Word)	VALUES ('used');
INSERT INTO cy_StopWords (Word)	VALUES ('using');
INSERT INTO cy_StopWords (Word)	VALUES ('very');
INSERT INTO cy_StopWords (Word)	VALUES ('was');
INSERT INTO cy_StopWords (Word)	VALUES ('we');
INSERT INTO cy_StopWords (Word)	VALUES ('well');
INSERT INTO cy_StopWords (Word)	VALUES ('what');
INSERT INTO cy_StopWords (Word)	VALUES ('when');
INSERT INTO cy_StopWords (Word)	VALUES ('where');
INSERT INTO cy_StopWords (Word)	VALUES ('who');
INSERT INTO cy_StopWords (Word)	VALUES ('will');
INSERT INTO cy_StopWords (Word)	VALUES ('with');
INSERT INTO cy_StopWords (Word)	VALUES ('www');
INSERT INTO cy_StopWords (Word)	VALUES ('you');
INSERT INTO cy_StopWords (Word)	VALUES ('your');

DECLARE @postID uniqueidentifier, @catID UNIQUEIDENTIFIER,@userID UNIQUEIDENTIFIER,@roleID UNIQUEIDENTIFIER;

SET @postID = NEWID();
SET @catID = NEWID();
SET @userID=NEWID();
SET @roleID=NEWID();

INSERT INTO cy_Categories (Id, [Name])
	VALUES (@catID, 'General');

INSERT INTO cy_Posts (Id, Title, Description, PostContent, DateCreated, Author, IsPublished)
	VALUES (@postID, 
	'Welcome to CoyoEden.NET 1.5 using Microsoft SQL Server', 
	'The description is used as the meta description as well as shown in the related posts. It is recommended that you write a description, but not mandatory',
	'<p>If you see this post it means that CoyoEden.NET 1.5 is running with SQL Server and the DbBlogProvider is configured correctly.</p>
	<h2>Setup</h2>
	<p>If you are using the ASP.NET Membership provider, you are set to use existing users.  If you are using the default CoyoEden.NET XML provider, it is time to setup some users.  Find the sign-in link located either at the bottom or top of the page depending on your current theme and click it. Now enter "admin" in both the username and password fields and click the button. You will now see an admin menu appear. It has a link to the "Users" admin page. From there you can change the username and password.</p>
	<h2>Write permissions</h2>
	<p>Since you are using SQL to store your posts, most information is stored there.  However, if you want to store attachments or images in the blog, you will want write permissions setup on the App_Data folder.</p>
	<h2>On the web </h2>
	<p>You can find CoyoEden.NET on the <a href="http://www.dotnetCoyoEden.net">official website</a>. Here you will find tutorials, documentation, tips and tricks and much more. The ongoing development of CoyoEden.NET can be followed at <a href="http://www.codeplex.com/CoyoEden">CodePlex</a> where the daily builds will be published for anyone to download.</p>
	<p>Good luck and happy writing.</p>
	<p>The CoyoEden.NET team</p>',
	GETDATE(), 
	'admin',
	1);

INSERT INTO cy_PostCategory (PostID, CategoryID)
	VALUES (@postID, @catID);
INSERT INTO cy_PostTag (PostID, Tag)
	VALUES (@postID, 'blog');
INSERT INTO cy_PostTag (PostID, Tag)
	VALUES (@postID, 'welcome');

INSERT INTO cy_Users (Id,UserName, [Password], LastLoginTime, EmailAddress)
	VALUES (@userID,'Admin', '', GETDATE(), 'email@example.com');
INSERT INTO cy_Roles ([Id],[Name]) 
	VALUES (@roleID,'Administrators');
INSERT INTO cy_Roles ([Name]) 
	VALUES ('Editors');
INSERT INTO cy_UserRoles (UserID, RoleID)
VALUES (@userID, @roleID);

GO
