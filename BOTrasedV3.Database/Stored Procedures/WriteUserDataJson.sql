CREATE PROCEDURE [dbo].[WriteUserDataJson]
	@userData NVARCHAR(MAX)
AS
	MERGE INTO UserData AS target
	USING (
		SELECT 
			Id,
			ExperiencePoints,
			Level,
			Money
		FROM 
	)
RETURN 0
