CREATE PROCEDURE UpdateBasketItem
    @UserID INT,
    @ProductID INT,
    @Amount INT
AS
BEGIN
    UPDATE BasketPositions
    SET Amount = @Amount
    WHERE UserID = @UserID AND ProductID = @ProductID;
END
GO
