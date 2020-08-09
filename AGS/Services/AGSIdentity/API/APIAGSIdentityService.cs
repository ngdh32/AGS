using System;
using System.Collections.Generic;
using System.Net.Http;
using AGSCommon.Models.EntityModels.AGSIdentity;
using Microsoft.AspNetCore.Http;
using AGSCommon.Models.EntityModels.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using AGSCommon.Models.ViewModels.AGSIdentity;
using AGS.Services.AGS.CurrentUser;
using System.Threading.Tasks;

namespace AGS.Services.AGSIdentity.API
{
    public class APIAGSIdentityService : IAGSIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private ICurrentUserService _currentUserService { get; set; }

        public APIAGSIdentityService(IHttpClientFactory clientFactory, ICurrentUserService currentUserService)
        {
            _clientFactory = clientFactory;
            _currentUserService = currentUserService;
        }

        public async Task<List<AGSFunctionClaimEntity>> GetFunctionClaimEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync("functionclaims").Result;
            return await HandleHttpResponse<List<AGSFunctionClaimEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public async Task<List<AGSUserEntity>> GetAGSUserEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync("users").Result;
            return await HandleHttpResponse<List<AGSUserEntity>>(responseMessage, ResponseDataType.JArray);
        }

        private HttpClient GetAGSIdentityClient()
        {
            var result = _clientFactory.CreateClient(AGSCommon.CommonConstant.AGSConstant.ags_identity_httpclient_name);
            var accessToken = _currentUserService.GetAccessToken();
            result.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            return result;
        }

        public async Task<AGSUserEntity> GetAGSUserEntity(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"users/{userId}").Result;
            return await HandleHttpResponse<AGSUserEntity>(responseMessage, ResponseDataType.JObject);
        }

        public async Task<List<AGSGroupEntity>> GetUserGroups(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"users/{userId}/groups").Result;
            return await HandleHttpResponse<List<AGSGroupEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public async Task<List<AGSGroupEntity>> GetAGSGroupEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups").Result;
            return await HandleHttpResponse<List<AGSGroupEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public async Task<bool> UpdateAGSUserEntity(AGSUserEntity userEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(userEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PutAsync($"users/{userEntity.Id}", data).Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        private async Task<T> HandleHttpResponse<T>(HttpResponseMessage responseMessage, ResponseDataType responseDataType)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                string resposneContent = await responseMessage.Content.ReadAsStringAsync();
                AGSResponse response = JsonConvert.DeserializeObject<AGSResponse>(resposneContent);
                if (response.Code == AGSResponse.ResponseCodeEnum.Done)
                {
                    switch (responseDataType)
                    {
                        case ResponseDataType.JArray:
                            var resultJArray = JArray.FromObject(response.Data);
                            return resultJArray.ToObject<T>();
                        case ResponseDataType.JObject:
                            var resultJObject = JObject.FromObject(response.Data);
                            return resultJObject.ToObject<T>();
                        case ResponseDataType.PrimitiveType:
                            //var resultJObjectPrimitive = JObject.FromObject(response.Data);
                            return (T)(object)response.Data;
                        case ResponseDataType.BooleanType:
                            return (T)(object)true;
                        default:
                            throw new ArgumentException();
                    }
                }
                else
                {
                    throw new AGSException(response.Code);
                }

            }
            else
            {
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new AGSException(AGSResponse.ResponseCodeEnum.NoPermissionError);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new AGSException(AGSResponse.ResponseCodeEnum.TokenExpiredError);
                }
                else
                {
                    throw new AGSException(AGSResponse.ResponseCodeEnum.UnknownError);
                }
            }
        }

        public async Task<bool> DeleteAGSUserEntity(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.DeleteAsync($"users/{userId}").Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public async Task<string> AddAGSUserEntity(AGSUserEntity userEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(userEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"users", data).Result;
            return await HandleHttpResponse<string>(responseMessage, ResponseDataType.PrimitiveType);
        }

        public async Task<AGSGroupEntity> GetAGSGroupEntity(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups/{groupId}").Result;
            return await HandleHttpResponse<AGSGroupEntity>(responseMessage, ResponseDataType.JObject);
        }

        public async Task<bool> UpdateAGSGroupEntity(AGSGroupEntity groupEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(groupEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PutAsync($"groups/{groupEntity.Id}", data).Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public async Task<List<AGSFunctionClaimEntity>> GetGroupFunctionClaims(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups/{groupId}/functionclaims").Result;
            return await HandleHttpResponse<List<AGSFunctionClaimEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public async Task<bool> DeleteAGSGroupEntity(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.DeleteAsync($"groups/{groupId}").Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public async Task<string> AddAGSGroupEntity(AGSGroupEntity groupEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(groupEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"groups", data).Result;
            return await HandleHttpResponse<string>(responseMessage, ResponseDataType.PrimitiveType);
        }

        public async Task<bool> ChangePassword(ChangeUserPasswordViewModel changeUserPasswordViewModel)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(changeUserPasswordViewModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"users/{changeUserPasswordViewModel.UserId}/changepw", data).Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.PrimitiveType);
        }

        public async Task<bool> ResetPassword(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.PostAsync($"users/{userId}/resetpw", null).Result;
            return await HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public enum ResponseDataType
        {
           JObject,
           JArray,
           PrimitiveType,
           BooleanType
        }
    }
}
