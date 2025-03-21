CREATE VIEW View_ProductsWithGroups AS
SELECT 
    p.ID AS ProductID,
    p.Name,
    p.Price,
    p.IsActive,
    g.ID AS GroupID,
    g.Name AS GroupName,
    pg.Name AS ParentGroupName
FROM Products p
LEFT JOIN ProductGroups g ON p.GroupID = g.ID
LEFT JOIN ProductGroups pg ON g.ParentID = pg.ID;
GO
