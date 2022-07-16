using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AGSDocumentCore.Models.DTOs.Services
{
    public class AGSUserViewModel
    {
        [JsonProperty("id")]
        public string UserId { get; init; }
        [JsonProperty("username")]
        public string Username { get; init; }
        [JsonProperty("departmentIds")]
        public List<string> Departments { get; init; }
    }

    public class AGSDepartmentViewModel
    {
        [JsonProperty("id")]
        public string DepartmentId { get; init; }
        [JsonProperty("Name")]
        public string DepartmentName {get; init;}
    }
}
