CREATE VIEW completed_time AS
 SELECT projects.project_id,
    min(projects."timestamp") AS min_timestamp
   FROM projects
  WHERE ((projects.phase) = 'completed')
  GROUP BY projects.project_id;