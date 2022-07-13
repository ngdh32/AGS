using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AGSDocumentCore.Repositories
{
    public class AGSUserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _agsIdentityUrl;
        private readonly Dictionary<string, string> _credentials = new Dictionary<string, string>();

        public AGSUserRepository(IConfiguration configuration)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            _httpClient = new HttpClient(httpClientHandler);
            _agsIdentityUrl = configuration["AGSIdentity:Url"];
            _credentials.Add("client_id", configuration["AGSIdentity:ClientId"]);
            _credentials.Add("client_secret", configuration["AGSIdentity:ClientSecret"]);
            _credentials.Add("grant_type", configuration["AGSIdentity:GrantType"]);
            _credentials.Add("scope", configuration["AGSIdentity:Scope"]);
        }

        public async Task<List<AGSUserViewModel>> GetUsers()
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
                    JObject tokenResponse = JObject.Parse(await accessTokenResponse.Content.ReadAsStringAsync());
                    HttpRequestMessage usersRequest = new HttpRequestMessage(HttpMethod.Get, $"{_agsIdentityUrl}/api/v1/users");
                    usersRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse["access_token"].ToString());
                    var usersResponse = await _httpClient.SendAsync(usersRequest);
                    if (!usersResponse.IsSuccessStatusCode)
                    {
                        var errorMessage = await usersResponse.Content.ReadAsStringAsync();;
                        throw new Exception(errorMessage);
                    }
                    var responseString = await usersResponse.Content.ReadAsStringAsync();
                    var userResult = JsonConvert.DeserializeObject<AGSUserResponse<List<AGSUserViewModel>>>(responseString);
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
