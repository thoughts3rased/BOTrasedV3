CREATE PROCEDURE [dbo].[LogCommandUsage]
	@commandName VARCHAR
AS
	MERGE INTO CommandUsageCounts AS Target
    USING (SELECT @commandName AS CommandName) AS Source
    ON (Target.Name = Source.CommandName)
    WHEN MATCHED THEN
        UPDATE SET UsageCount = Target.UsageCount + 1
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Name, UsageCount)
        VALUES (Source.CommandName, 1);
RETURN 0
