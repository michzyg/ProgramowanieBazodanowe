CREATE VIEW View_OrderDetails AS
SELECT 
    op.OrderID,
    p.Name AS ProductName,
    op.Price,
    op.Amount,
    (op.Price * op.Amount) AS TotalValue
FROM OrderPositions op
JOIN Products p ON op.ProductId = p.ID;
GO
