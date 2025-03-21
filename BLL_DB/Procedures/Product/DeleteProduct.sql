CREATE PROCEDURE DeleteProduct
    @ProductID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Products WHERE ID = @ProductID)
    BEGIN
        RAISERROR('Product not found.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM OrderPositions WHERE ProductId = @ProductID)
    BEGIN
        RAISERROR('Cannot delete a product linked to order items.', 16, 1);
        RETURN;
    END

    DELETE FROM Products WHERE ID = @ProductID;
END
GO
