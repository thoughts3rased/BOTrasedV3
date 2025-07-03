CREATE TABLE [dbo].[Servers]
(
	[Id] CHAR(19) NOT NULL PRIMARY KEY, 
    [levelUpMessageEnabled] BIT NOT NULL DEFAULT 0, 
    [lockdownModeEnabled] BIT NOT NULL DEFAULT 0, 
    [autoRoleEnabled] BIT NOT NULL DEFAULT 0, 
    [autoRoleId] CHAR(19) NULL
)
