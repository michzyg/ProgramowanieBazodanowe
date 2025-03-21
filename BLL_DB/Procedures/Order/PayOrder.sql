CREATE PROCEDURE PayOrder
    @OrderID INT,
    @Amount DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalValue DECIMAL(18,2);

    IF NOT EXISTS (SELECT 1 FROM Orders WHERE ID = @OrderID)
    BEGIN
        SELECT -1; -- Not found
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Orders WHERE ID = @OrderID AND IsPaid = 1)
    BEGIN
        SELECT -2; -- Already paid
        RETURN;
    END

    SELECT @TotalValue = SUM(op.Price * op.Amount)
    FROM OrderPositions op
    WHERE op.OrderID = @OrderID;

    IF ABS(@Amount - @TotalValue) > 0.01
    BEGIN
        SELECT -3; -- Mismatch
        RETURN;
    END

    UPDATE Orders
    SET IsPaid = 1
    WHERE ID = @OrderID;

    SELECT 1; -- Success
END
GO
