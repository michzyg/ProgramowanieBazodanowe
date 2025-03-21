CREATE VIEW View_ProductsWithFullGroupHierarchy AS
WITH GroupHierarchy AS (
    SELECT 
        ID,
        Name,
        ParentID,
        CAST(Name AS NVARCHAR(MAX)) AS FullPath
    FROM ProductGroups
    WHERE ParentID IS NULL

    UNION ALL

    SELECT 
        g.ID,
        g.Name,
        g.ParentID,
        CAST(gh.FullPath + ' / ' + g.Name AS NVARCHAR(MAX))
    FROM ProductGroups g
    JOIN GroupHierarchy gh ON g.ParentID = gh.ID
)
SELECT 
    p.ID AS ProductID,
    p.Name,
    p.Price,
    p.IsActive,
    p.GroupID,
    gh.FullPath AS GroupName
FROM Products p
LEFT JOIN GroupHierarchy gh ON p.GroupID = gh.ID;
GO
