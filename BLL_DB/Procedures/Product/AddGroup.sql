CREATE PROCEDURE AddGroup
    @Name NVARCHAR(100),
    @ParentID INT = NULL
AS
BEGIN
    INSERT INTO ProductGroups (Name, ParentID)
    VALUES (@Name, @ParentID);
END
GO
