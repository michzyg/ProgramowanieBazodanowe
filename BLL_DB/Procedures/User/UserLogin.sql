CREATE PROCEDURE UserLogin
    @Login NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM Users
        WHERE Login = @Login AND Password = @Password AND IsActive = 1
    )
    BEGIN
        SELECT 1; -- Success
    END
    ELSE
    BEGIN
        SELECT 0;  -- Failed login
    END
END
GO
