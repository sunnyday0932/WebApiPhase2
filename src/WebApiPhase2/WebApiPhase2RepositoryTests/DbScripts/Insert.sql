SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Idx], [Account], [Password], [Phone], [Email], [CreateDate], [ModifyDate], [ModifyUser]) VALUES (2, N'test1', N'qC5MYUAvwVITJyWKfxIH1PjpeS7G2748+s6DTMUJjEI=', N'0988123111', N'test1@gmail.com', CAST(N'2020-12-21T21:43:46.667' AS DateTime), CAST(N'2020-12-22T21:57:13.120' AS DateTime), N'test1')
INSERT [dbo].[Users] ([Idx], [Account], [Password], [Phone], [Email], [CreateDate], [ModifyDate], [ModifyUser]) VALUES (3, N'test2', N'9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=', N'0988123456', N'test22@gmail.com', CAST(N'2021-01-23T12:08:10.727' AS DateTime), CAST(N'2021-01-23T13:58:21.883' AS DateTime), N'test2')
SET IDENTITY_INSERT [dbo].[Users] OFF
