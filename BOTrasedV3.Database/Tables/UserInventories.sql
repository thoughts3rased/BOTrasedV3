CREATE TABLE [dbo].[UserInventories]
(
	[UserId] CHAR(18) NOT NULL , 
    [ItemId] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 0, 
    [ShowOnProfile] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY ([ItemId], [UserId]), 
    CONSTRAINT [FK_UserInventories_Users] FOREIGN KEY (UserId) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_UserInventories_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([Id])
)
