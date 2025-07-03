CREATE TABLE [dbo].[CommandUsageCounts]
(
	[Name] VARCHAR(255) NOT NULL PRIMARY KEY, 
    [UsageCount] BIGINT NOT NULL DEFAULT 0 
)
