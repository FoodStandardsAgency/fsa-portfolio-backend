

CREATE VIEW updates_int3 AS
 SELECT updates_int2.project_id,
    updates_int2."timestamp",
    convert(date, updates_int2."timestamp") AS date,
    updates_int2.[update]
   FROM updates_int2
  WHERE ((updates_int2."timestamp" = updates_int2.min_timestamp) AND (updates_int2.[update] IS NOT NULL));