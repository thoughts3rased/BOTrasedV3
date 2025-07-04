CREATE PROCEDURE [dbo].[GetRandomStatus]
AS
	SELECT TOP 1
		Id, 
	    StatusText, 
	    ActivityType
	FROM BotStatuses
	ORDER BY NEWID()
	FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
RETURN 0
