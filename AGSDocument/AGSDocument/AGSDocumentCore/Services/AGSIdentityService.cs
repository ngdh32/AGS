using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AGSDocumentCore.Services
{
    public class AGSIdentityService : IAGSIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _agsIdentityUrl;
        private readonly Dictionary<string, string> _credentials = new Dictionary<string, string>();

        public AGSIdentityService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _agsIdentityUrl = configuration["AGSIdentityUrl"];
            _credentials.Add("client_id", configuration["AGSIdentityClientId"]);
            _credentials.Add("client_secret", configuration["AGSIdentityClientSecret"]);
            _credentials.Add("grant_type", configuration["AGSIdentityGrantType"]);
            _credentials.Add("scope", configuration["AGSIdentityScope"]);
        }

        public async Task<List<AGSUser>> GetUsers()
        {
            HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{_agsIdentityUrl}/connect/token");

            // get the access token
            var formData = new List<KeyValuePair<string, string>>();
            foreach(var credential in _credentials)
            {
                formData.Add(new KeyValuePair<string, string>(credential.Key, credential.Value));
            }
            var requestContent = new FormUrlEncodedContent(formData);
            tokenRequest.Content = requestContent;
            try
            {
                var accessTokenResponse = await _httpClient.SendAsync(tokenRequest);
                if (accessTokenResponse.IsSuccessStatusCode)
                {
                    dynamic tokenResponse = JsonConvert.DeserializeObject(await accessTokenResponse.Content.ReadAsStringAsync());
                    HttpRequestMessage usersRequest = new HttpRequestMessage(HttpMethod.Get, $"{_agsIdentityUrl}/api/v1/users");
                    usersRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.access_token);
                    var usersResponse = await _httpClient.SendAsync(usersRequest);
                    var userResult = JsonConvert.DeserializeObject<AGSUserResponse<List<AGSUser>>>(await usersResponse.Content.ReadAsStringAsync());
                    return userResult.Data;
                }
                else
                {
                    var errorMessage = await accessTokenResponse.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);
                }
            }
            catch(Exception ex)
            {
                throw;
            }

        }
    }
}
