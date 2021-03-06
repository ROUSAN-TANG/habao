USE [master]
GO
/****** Object:  Database [db_LANGobang]    Script Date: 2016/7/4 11:33:27 ******/
CREATE DATABASE [db_LANGobang]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db_LANGobang_Data', FILENAME = N'F:\Program Files (x86)\Microsoft SQL Server\MSSQL12.MR2014\MSSQL\DATA\db_LANGobang.MDF' , SIZE = 2688KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'db_LANGobang_Log', FILENAME = N'F:\Program Files (x86)\Microsoft SQL Server\MSSQL12.MR2014\MSSQL\DATA\db_LANGobang_Log.LDF' , SIZE = 3456KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE [db_LANGobang] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [db_LANGobang].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [db_LANGobang] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [db_LANGobang] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [db_LANGobang] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [db_LANGobang] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [db_LANGobang] SET ARITHABORT OFF 
GO
ALTER DATABASE [db_LANGobang] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [db_LANGobang] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [db_LANGobang] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [db_LANGobang] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [db_LANGobang] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [db_LANGobang] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [db_LANGobang] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [db_LANGobang] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [db_LANGobang] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [db_LANGobang] SET  DISABLE_BROKER 
GO
ALTER DATABASE [db_LANGobang] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [db_LANGobang] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [db_LANGobang] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [db_LANGobang] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [db_LANGobang] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [db_LANGobang] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [db_LANGobang] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [db_LANGobang] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [db_LANGobang] SET  MULTI_USER 
GO
ALTER DATABASE [db_LANGobang] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [db_LANGobang] SET DB_CHAINING OFF 
GO
ALTER DATABASE [db_LANGobang] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [db_LANGobang] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [db_LANGobang] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'db_LANGobang', N'ON'
GO
USE [db_LANGobang]
GO
/****** Object:  Table [dbo].[tb_Gobang]    Script Date: 2016/7/4 11:33:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_Gobang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IP] [varchar](20) NULL,
	[Port] [varchar](10) NULL,
	[UserName] [varchar](50) NULL,
	[PassWord] [varchar](50) NULL,
	[Fraction] [int] NULL CONSTRAINT [DF_tb_Gobang_Fraction]  DEFAULT (2000),
	[State] [int] NULL CONSTRAINT [DF_tb_Gobang_State]  DEFAULT (0),
	[Borough] [int] NULL CONSTRAINT [DF_tb_Gobang_Area]  DEFAULT (0),
	[RoomMark] [int] NULL CONSTRAINT [DF_tb_Gobang_Apartment]  DEFAULT (0),
	[DeskMark] [varchar](20) NULL CONSTRAINT [DF_tb_Gobang_DeskMark]  DEFAULT (0),
	[SeatMark] [varchar](20) NULL CONSTRAINT [DF_tb_Gobang_SeatMark]  DEFAULT (0),
	[UserCaption] [varchar](20) NULL CONSTRAINT [DF_tb_Gobang_UserCaption]  DEFAULT (0),
	[Caput] [int] NULL CONSTRAINT [DF_tb_Gobang_Caput]  DEFAULT (0),
	[Sex] [int] NULL CONSTRAINT [DF_tb_Gobang_Sex]  DEFAULT (0),
 CONSTRAINT [PK_tb_Gobang] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Gobang] ON 

INSERT [dbo].[tb_Gobang] ([ID], [IP], [Port], [UserName], [PassWord], [Fraction], [State], [Borough], [RoomMark], [DeskMark], [SeatMark], [UserCaption], [Caput], [Sex]) VALUES (64, N'192.168.1.5', N'11025', N'蓝色海洋', N'811129x', 2000, 30, 0, 0, N'0', N'0', N'label1_2', 3, 0)
INSERT [dbo].[tb_Gobang] ([ID], [IP], [Port], [UserName], [PassWord], [Fraction], [State], [Borough], [RoomMark], [DeskMark], [SeatMark], [UserCaption], [Caput], [Sex]) VALUES (65, N'192.168.1.2', N'11026', N'123', N'123', 2000, 30, 0, 0, N'0', N'0', N'label2_1', 0, 0)
INSERT [dbo].[tb_Gobang] ([ID], [IP], [Port], [UserName], [PassWord], [Fraction], [State], [Borough], [RoomMark], [DeskMark], [SeatMark], [UserCaption], [Caput], [Sex]) VALUES (66, N'192.168.1.41', N'1245', N'lx', N'lx', 1910, 30, 0, 0, N'0', N'0', N'label22_2', 0, 1)
INSERT [dbo].[tb_Gobang] ([ID], [IP], [Port], [UserName], [PassWord], [Fraction], [State], [Borough], [RoomMark], [DeskMark], [SeatMark], [UserCaption], [Caput], [Sex]) VALUES (67, N'192.168.1.230', N'12555', N'LB', N'1', 2090, 30, 0, 0, N'0', N'0', N'label23_1', 3, 0)
SET IDENTITY_INSERT [dbo].[tb_Gobang] OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'IP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'PassWord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'Fraction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户当前状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'Borough'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'房间号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'RoomMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'桌号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'DeskMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'坐位号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'SeatMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户坐位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'UserCaption'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'Caput'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_Gobang', @level2type=N'COLUMN',@level2name=N'Sex'
GO
USE [master]
GO
ALTER DATABASE [db_LANGobang] SET  READ_WRITE 
GO
