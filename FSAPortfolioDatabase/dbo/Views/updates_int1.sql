
CREATE VIEW updates_int1 AS
 SELECT projects.project_id,
    projects.[update],
    min(projects."timestamp") AS min_timestamp
   FROM projects
  GROUP BY projects.project_id, projects.[update];