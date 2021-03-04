/****** Object:  Table [dbo].[Users]    Script Date: 2021/3/1 ¤W¤È 09:39:27 ******/
CREATE TABLE [dbo].[Users](
	[Idx] [int] IDENTITY(1,1) NOT NULL,
	[Account] [varchar](30) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Phone] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyUser] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



