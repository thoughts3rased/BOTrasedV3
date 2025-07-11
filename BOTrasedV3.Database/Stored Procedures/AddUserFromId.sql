CREATE PROCEDURE [dbo].[AddUserFromId]
	@userId CHAR(18)
AS
	INSERT INTO Users (Id)
	SELECT @userId
	WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = @userId);
