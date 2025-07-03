CREATE TABLE [dbo].[Items]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Type] INT NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Purchasable] BIT NOT NULL DEFAULT 0, 
    [Price] INT NOT NULL DEFAULT 0, 
    [Description] VARCHAR(MAX) NULL
)
