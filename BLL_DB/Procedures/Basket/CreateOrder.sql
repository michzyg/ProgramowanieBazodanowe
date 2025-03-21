CREATE PROCEDURE CreateOrder
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM BasketPositions WHERE UserID = @UserID)
    BEGIN
        RAISERROR('Basket is empty.', 16, 1);
        RETURN;
    END

    DECLARE @OrderID INT;

    INSERT INTO Orders (UserID, Date, IsPaid)
    VALUES (@UserID, GETDATE(), 0);

    SET @OrderID = SCOPE_IDENTITY();

    INSERT INTO OrderPositions (OrderID, ProductId, Amount, Price)
    SELECT @OrderID, bp.ProductID, bp.Amount, p.Price
    FROM BasketPositions bp
    JOIN Products p ON bp.ProductID = p.ID
    WHERE bp.UserID = @UserID;

    DELETE FROM BasketPositions WHERE UserID = @UserID;

    SELECT o.ID AS OrderID, o.Date AS OrderDate, o.IsPaid,
           SUM(op.Price * op.Amount) AS TotalPrice
    FROM Orders o
    JOIN OrderPositions op ON o.ID = op.OrderID
    WHERE o.ID = @OrderID
    GROUP BY o.ID, o.Date, o.IsPaid;
END
GO
