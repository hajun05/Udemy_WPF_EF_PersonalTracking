USE [PERSONALTRACKING]
GO
/****** Object:  Table [dbo].[DEPARTMENT]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DEPARTMENT](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPLOYEE]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLOYEE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserNo] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[ImagePath] [varchar](max) NOT NULL,
	[DepartmentID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
	[Salary] [int] NOT NULL,
	[BirtyDay] [date] NULL,
	[Address] [varchar](max) NULL,
	[Password] [varchar](20) NULL,
	[isAdmin] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PERMISSION]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERMISSION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[PermissionStartDate] [date] NOT NULL,
	[PermissionEndDate] [date] NOT NULL,
	[PermissionState] [int] NOT NULL,
	[PermissionExplanation] [varchar](max) NULL,
	[PermissionDay] [int] NOT NULL,
	[UserNo] [int] NOT NULL,
	[PermissionAmount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PERMISSIONSTATE]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERMISSIONSTATE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StateName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POSITION]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POSITION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PositionName] [varchar](50) NOT NULL,
	[DepartmentId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SALARY]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SALARY](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[MonthId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SALARYMONTH]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SALARYMONTH](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MonthName] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASK]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASK](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[TaskTitle] [varchar](50) NOT NULL,
	[TaskContent] [varchar](max) NULL,
	[TaskStartDate] [date] NULL,
	[TaskDeliveryDate] [date] NULL,
	[TaskState] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASKSTATE]    Script Date: 2025-06-24 오후 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASKSTATE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StateName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEE_DEPARTMENT] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[DEPARTMENT] ([ID])
GO
ALTER TABLE [dbo].[EMPLOYEE] CHECK CONSTRAINT [FK_EMPLOYEE_DEPARTMENT]
GO
ALTER TABLE [dbo].[EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEE_POSITION] FOREIGN KEY([PositionID])
REFERENCES [dbo].[POSITION] ([ID])
GO
ALTER TABLE [dbo].[EMPLOYEE] CHECK CONSTRAINT [FK_EMPLOYEE_POSITION]
GO
ALTER TABLE [dbo].[PERMISSION]  WITH CHECK ADD  CONSTRAINT [FK_PERMISSION_EMPLOYEE] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[EMPLOYEE] ([ID])
GO
ALTER TABLE [dbo].[PERMISSION] CHECK CONSTRAINT [FK_PERMISSION_EMPLOYEE]
GO
ALTER TABLE [dbo].[PERMISSION]  WITH CHECK ADD  CONSTRAINT [FK_PERMISSION_PERMISSIONSTATE] FOREIGN KEY([PermissionState])
REFERENCES [dbo].[PERMISSIONSTATE] ([ID])
GO
ALTER TABLE [dbo].[PERMISSION] CHECK CONSTRAINT [FK_PERMISSION_PERMISSIONSTATE]
GO
ALTER TABLE [dbo].[POSITION]  WITH CHECK ADD  CONSTRAINT [FK_POSITION_DEPARTMENT] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[DEPARTMENT] ([ID])
GO
ALTER TABLE [dbo].[POSITION] CHECK CONSTRAINT [FK_POSITION_DEPARTMENT]
GO
ALTER TABLE [dbo].[SALARY]  WITH CHECK ADD  CONSTRAINT [FK_SALARY_EMPLOYEE] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[EMPLOYEE] ([ID])
GO
ALTER TABLE [dbo].[SALARY] CHECK CONSTRAINT [FK_SALARY_EMPLOYEE]
GO
ALTER TABLE [dbo].[SALARY]  WITH CHECK ADD  CONSTRAINT [FK_SALARY_SALARYMONTH] FOREIGN KEY([MonthId])
REFERENCES [dbo].[SALARYMONTH] ([ID])
GO
ALTER TABLE [dbo].[SALARY] CHECK CONSTRAINT [FK_SALARY_SALARYMONTH]
GO
ALTER TABLE [dbo].[TASK]  WITH CHECK ADD  CONSTRAINT [FK_TASK_EMPLOYEE] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[EMPLOYEE] ([ID])
GO
ALTER TABLE [dbo].[TASK] CHECK CONSTRAINT [FK_TASK_EMPLOYEE]
GO
ALTER TABLE [dbo].[TASK]  WITH CHECK ADD  CONSTRAINT [FK_TASK_TASKSTATE] FOREIGN KEY([TaskState])
REFERENCES [dbo].[TASKSTATE] ([ID])
GO
ALTER TABLE [dbo].[TASK] CHECK CONSTRAINT [FK_TASK_TASKSTATE]
GO
