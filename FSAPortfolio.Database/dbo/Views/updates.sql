

CREATE VIEW updates AS
 SELECT updates_int5."timestamp",
    updates_int5.max_timestamp,
    updates_int5.project_id,
    updates_int5.date,
    updates_int5.[update]
   FROM updates_int5
  WHERE (updates_int5."timestamp" = updates_int5.max_timestamp);