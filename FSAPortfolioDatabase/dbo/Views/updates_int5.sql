

CREATE VIEW updates_int5 AS
 SELECT a."timestamp",
    a.project_id,
    a.date,
    a.[update],
    b.max_timestamp
   FROM (updates_int3 a
     JOIN updates_int4 b ON ((((a.project_id) = (b.project_id)) AND (a.date = b.date))));