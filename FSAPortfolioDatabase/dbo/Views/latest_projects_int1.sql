
CREATE VIEW latest_projects_int1 AS
 SELECT projects.project_id,
    max(projects."timestamp") AS max_time,
    min(projects."timestamp") AS min_time
   FROM projects
  GROUP BY projects.project_id;