﻿using FSAPortfolio.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.DTO
{
    public class GetProjectQueryDTO
    {
        [JsonProperty("config")]
        public ProjectLabelConfigModel Config { get; set; }

        [JsonProperty("options")]
        public ProjectEditOptionsModel Options { get; set; }

    }
}