﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class UserRequestModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }
    }

    public class UserModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("accessGroup")]
        public string AccessGroup { get; set; }
    }

    public class AddSupplierModel
    {
        [JsonProperty("portfolio")]
        public string Portfolio { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }
    }

    public class SupplierResponseModel
    {
        [JsonProperty("suppliers")]
        public IEnumerable<string> Suppliers { get; set; }

    }
    public class AddSupplierResponseModel
    {
        public string result { get; set; }
    }

    public class AddUserModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("accessgroup")]
        public string AccessGroup { get; set; }
    }
}