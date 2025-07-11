CREATE TABLE [dbo].[Users]
(
	[Id] CHAR(18) NOT NULL PRIMARY KEY, 
    [ExperiencePoints] BIGINT NOT NULL DEFAULT 0, 
    [Level] INT NOT NULL DEFAULT 0, 
    [Money] INT NOT NULL DEFAULT 0, 
    [LevelUpMessage] BIT NOT NULL DEFAULT 1
)
