CREATE TABLE [dbo].[BotStatuses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StatusText] VARCHAR(30) NOT NULL, 
    [ActivityType] INT NOT NULL DEFAULT 0
)
