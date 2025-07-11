CREATE PROCEDURE [dbo].[WriteUserJson]
	@json NVARCHAR(MAX) = 0
AS
	MERGE INTO Users AS Target
	USING (
		SELECT * FROM OPENJSON(@json)
		WITH (
			Id CHAR(18) '$.Id',
			ExperiencePoints BIGINT '$.ExperiencePoints',
			Level INT '$.Level',
			Money INT '$.Money',
			LevelUpMessage BIT '$.LevelUpMessage'
		)
	) AS Source
	ON (Target.Id = Source.Id)
	WHEN MATCHED THEN
		UPDATE SET 
		Target.LevelUpMessage = Source.LevelUpMessage
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Id, ExperiencePoints, Level, Money, LevelUpMessage)
		VALUES (Source.Id, 0, 0, 0, COALESCE(Source.LevelUpMessage, 1));