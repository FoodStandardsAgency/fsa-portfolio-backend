﻿using FSAPortfolio.Common;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    /// <summary>
    /// This class forms the main contract between the front end and the API.
    /// The names of these fields are stored in the label configurations in the database.
    /// Take caution when renaming these fields as this is likely to break:
    /// - existing label configurations
    /// - front end views and forms
    /// </summary>
    public class ProjectModel
    {
        public string id { get; set; }

        /// <summary>
        /// This is the project identifier and is the unique key for a project.
        /// It is created automatically by the system when a new project is requested. It can't be changed or switched off.
        /// The format is based on the current month, for example: `ODD2007010`.
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        /// <summary>
        /// The concise name of the project, limited to a maximum of 250 characters.
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.project_name)]
        public string project_name { get; set; }

        [JsonProperty(ProjectPropertyConstants.short_desc)]
        public string short_desc { get; set; }


        /// <summary>
        /// This is the current *phase* for the project.
        /// A project moves through phases as it progresses from inception to completion. Phases are configured in the portfolio configuration:
        /// - The minimum number of phases is 2
        /// - The maximum number of phases is 6
        /// - The last configured phase is the *completed* phase:
        ///     - it is not shown in portfolio summaries
        /// - The last but one phase is the *archive* phase:
        ///     - this phase is the last phase shown in project summaries
        ///     - projects remain in this phase for 90 days
        ///     - after 90 days projects are automatically moved to the *completed* phase
        /// - If the portfolio phases are reconfigured:
        ///     - historic phases are not preserved
        ///     - projects in the *nth phase* will remain in the *nth phase* after configuration
        ///     - portfolio configuration shows an error if it would remove a phase that contains projects
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.phase), UpdateTracked]
        public string phase { get; set; }

        /// <summary>
        /// The selected Category, from a list of categories. These are configured independently for each portfolio.
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.category)]
        public string category { get; set; }

        /// <summary>
        /// The selected Sub-categories, multiple values can be selected from the list of configured categories.
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.subcat)]
        public string[] subcat { get; set; }

        /// <summary>
        /// The selected RAG status. Depending on the configuration, the value is chosen from either:
        /// - Red; Amber; Green (3 status options) or 
        /// - Red; Red Amber; Amber; Amber Green; Green (5 status options)
        /// </summary>
        [UpdateTracked]
        public string rag { get; set; }

        [UpdateTracked]
        public string update { get; set; }
        public string priority_main { get; set; }
        public int funded { get; set; }
        public int confidence { get; set; }
        public int priorities { get; set; }
        public int benefits { get; set; }
        public int criticality { get; set; }

        [UpdateTracked]
        public string budget { get; set; }

        [UpdateTracked]
        public string spent { get; set; }
        public LinkModel[] documents { get; set; }
        public DateTime? timestamp { get; set; }
        public string pgroup { get; set; }

        [JsonProperty(ProjectPropertyConstants.onhold), UpdateTracked]
        public string onhold { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_size)]
        public string project_size { get; set; }

        [JsonProperty(ProjectPropertyConstants.oddlead_role)]
        public string oddlead_role { get; set; }

        [JsonProperty(ProjectPropertyConstants.budgettype)]
        public string budgettype { get; set; }
        public string direct { get; set; }

        [UpdateTracked]
        public float? p_comp { get; set; }
        public DateTime? max_time { get; set; }
        public DateTime? min_time { get; set; }
        public string g6team { get; set; }
        public string new_flag { get; set; }
        public DateTime? first_completed { get; set; }

        public ProjectPersonModel key_contact1 { get; set; }
        public ProjectPersonModel key_contact2 { get; set; }
        public ProjectPersonModel key_contact3 { get; set; }


        // New fields
        [JsonProperty(ProjectPropertyConstants.business_case_number)]
        public string business_case_number { get; set; }

        [JsonProperty(ProjectPropertyConstants.fs_number)]
        public string fs_number { get; set; }

        [JsonProperty(ProjectPropertyConstants.risk_rating)]
        public string risk_rating { get; set; }

        [JsonProperty(ProjectPropertyConstants.programme_description)]
        public string programme_description { get; set; }

        [JsonProperty(ProjectPropertyConstants.link)]
        public LinkModel link { get; set; }


        public string project_team_setting1 { get; set; }
        public string project_team_setting2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_team_option1)]
        public string project_team_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_team_option2)]
        public string project_team_option2 { get; set; }

        public string project_plan_setting1 { get; set; }
        public string project_plan_setting2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_plan_option1)]
        public string project_plan_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_plan_option2)]
        public string project_plan_option2 { get; set; }

        public string progress_setting1 { get; set; }
        public string progress_setting2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.progress_option1)]
        public string progress_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.progress_option2)]
        public string progress_option2 { get; set; }

        public int budget_field1 { get; set; } // TODO: check! Existed prior to Project settings
        public int budget_field2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_option1)]
        public string budget_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_option2)]
        public string budget_option2 { get; set; }

        public string processes_setting1 { get; set; }
        public string processes_setting2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.processes_option1)]
        public string processes_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.processes_option2)]
        public string processes_option2 { get; set; }


        [JsonProperty(PropertyName = ProjectPropertyConstants.theme)]
        public string theme { get; set; }


        [JsonProperty(PropertyName = ProjectPropertyConstants.project_type)]
        public string project_type { get; set; }

        [JsonProperty(ProjectPropertyConstants.strategic_objectives)]
        public string strategic_objectives { get; set; }

        [JsonProperty(ProjectPropertyConstants.programme)]
        public string programme { get; set; }

        [JsonProperty(PropertyName = ProjectPropertyConstants.Supplier)]
        public string supplier { get; set; }

        public string how_get_green { get; set; }
        public string forward_look { get; set; }
        public string emerging_issues { get; set; }
        

        public int forecast_spend { get; set; }
        public string cost_centre { get; set; }
        public string fsaproc_assurance_gatenumber { get; set; }
        public string fsaproc_assurance_nextgate { get; set; }
    }

    public class LinkModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class UpdateHistoryModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class StatusUpdateHistoryModel
    {
        [JsonProperty("rag")]
        public string RAGStatus { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class UpdateTrackedAttribute : Attribute
    {

    }

}