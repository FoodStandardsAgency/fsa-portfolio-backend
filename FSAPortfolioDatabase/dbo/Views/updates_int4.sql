

CREATE VIEW updates_int4 AS
 SELECT updates_int3.project_id,
    updates_int3.date,
    max(updates_int3."timestamp") AS max_timestamp
   FROM updates_int3
  GROUP BY updates_int3.project_id, updates_int3.date;