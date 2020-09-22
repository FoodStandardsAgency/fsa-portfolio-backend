
CREATE VIEW updates_int2 AS
 SELECT a.project_id,
    a."timestamp",
    a.[update],
    b.min_timestamp
   FROM (projects a
     JOIN updates_int1 b ON (((a.project_id) = (b.project_id))));