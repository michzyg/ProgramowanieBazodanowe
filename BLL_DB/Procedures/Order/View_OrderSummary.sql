CREATE VIEW View_OrderSummary AS
SELECT 
    o.ID AS OrderID,
    o.Date AS OrderDate,
    o.IsPaid,
    SUM(op.Price * op.Amount) AS TotalPrice
FROM Orders o
JOIN OrderPositions op ON o.ID = op.OrderID
GROUP BY o.ID, o.Date, o.IsPaid;
GO
