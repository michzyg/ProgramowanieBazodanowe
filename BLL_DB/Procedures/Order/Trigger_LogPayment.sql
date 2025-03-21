CREATE TABLE OrderPaymentLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT,
    PaidAt DATETIME DEFAULT GETDATE()
);
GO

CREATE TRIGGER trg_LogOrderPaid
ON Orders
AFTER UPDATE
AS
BEGIN
    IF UPDATE(IsPaid)
    BEGIN
        INSERT INTO OrderPaymentLog (OrderID)
        SELECT ID
        FROM INSERTED
        WHERE IsPaid = 1;
    END
END
GO
