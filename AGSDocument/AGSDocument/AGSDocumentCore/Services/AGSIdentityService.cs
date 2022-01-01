using System;
using System.Collections.Generic;
using System.Net.Http;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using Microsoft.Extensions.Configuration;

namespace AGSDocumentCore.Services
{
    public class AGSIdentityService : IAGSIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _agsIdentityUrl;

        public AGSIdentityService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _agsIdentityUrl = configuration["AGSIdentityUrl"];

            // get the access token
            var formData = new List<KeyValuePair<string, string>>();
            //formData.Add(new KeyValuePair<string, string>("client_id", "TEST2"));
            //formData.Add(new KeyValuePair<string, string>("client_secret", "TEST2"));
            //formData.Add(new KeyValuePair<string, string>("grant_type", "TEST2"));
            //formData.Add(new KeyValuePair<string, string>("scope", "TEST2"));
            //formData.Add(new KeyValuePair<string, string>("username", "TEST2"));
            //formData.Add(new KeyValuePair<string, string>("password", "TEST2"));
        }

        public List<AGSUser> GetUsers()
        {
            
            
        }
    }
}
