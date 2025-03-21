CREATE VIEW View_ProductGroups AS
SELECT 
    g.ID AS Id,
    g.Name,
    g.ParentID,
    (SELECT COUNT(*) FROM ProductGroups sg WHERE sg.ParentID = g.ID) AS HasChildren
FROM ProductGroups g;
GO
