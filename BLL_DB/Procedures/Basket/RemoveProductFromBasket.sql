CREATE PROCEDURE RemoveProductFromBasket
    @UserID INT,
    @ProductID INT
AS
BEGIN
    DELETE FROM BasketPositions
    WHERE UserID = @UserID AND ProductID = @ProductID;
END
GO
