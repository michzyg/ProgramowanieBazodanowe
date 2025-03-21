CREATE PROCEDURE AddProductToBasket
    @UserID INT,
    @ProductID INT,
    @Amount INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM BasketPositions
        WHERE UserID = @UserID AND ProductID = @ProductID
    )
    BEGIN
        UPDATE BasketPositions
        SET Amount = Amount + @Amount
        WHERE UserID = @UserID AND ProductID = @ProductID;
    END
    ELSE
    BEGIN
        INSERT INTO BasketPositions (UserID, ProductID, Amount)
        VALUES (@UserID, @ProductID, @Amount);
    END
END
GO
