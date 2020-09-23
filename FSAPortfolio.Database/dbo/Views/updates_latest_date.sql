

CREATE VIEW updates_latest_date AS
 SELECT updates.project_id,
    max(updates."timestamp") AS latest_update
   FROM updates
  GROUP BY updates.project_id;