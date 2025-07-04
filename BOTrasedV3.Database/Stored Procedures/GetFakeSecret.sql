CREATE PROCEDURE GetRandomFakeSecret
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 
        Id, 
        Content
    FROM FakeSecrets
    ORDER BY NEWID()
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
END;

