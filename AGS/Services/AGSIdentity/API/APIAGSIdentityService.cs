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

namespace AGS.Services.AGSIdentity.API
{
    public class APIAGSIdentityService : IAGSIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        public APIAGSIdentityService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<AGSFunctionClaimEntity> GetFunctionClaimEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync("functionclaims").Result;
            return HandleHttpResponse<List<AGSFunctionClaimEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public List<AGSUserEntity> GetAGSUserEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync("users").Result;
            return HandleHttpResponse<List<AGSUserEntity>>(responseMessage, ResponseDataType.JArray);
        }

        private HttpClient GetAGSIdentityClient()
        {
            var result = _clientFactory.CreateClient(AGSCommon.CommonConstant.AGSConstant.ags_identity_httpclient_name);
            var accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            result.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            return result;
        }

        public AGSUserEntity GetAGSUserEntity(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"users/{userId}").Result;
            return HandleHttpResponse<AGSUserEntity>(responseMessage, ResponseDataType.JObject);
        }

        public List<AGSGroupEntity> GetUserGroups(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"users/{userId}/groups").Result;
            return HandleHttpResponse<List<AGSGroupEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public List<AGSGroupEntity> GetAGSGroupEntities()
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups").Result;
            return HandleHttpResponse<List<AGSGroupEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public bool UpdateAGSUserEntity(AGSUserEntity userEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(userEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PutAsync($"users/{userEntity.Id}", data).Result;
            return HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        private T HandleHttpResponse<T>(HttpResponseMessage responseMessage, ResponseDataType responseDataType)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                string resposneContent = responseMessage.Content.ReadAsStringAsync().Result;
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

        public bool DeleteAGSUserEntity(string userId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.DeleteAsync($"users/{userId}").Result;
            return HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public string AddAGSUserEntity(AGSUserEntity userEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(userEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"users", data).Result;
            return HandleHttpResponse<string>(responseMessage, ResponseDataType.PrimitiveType);
        }

        public AGSGroupEntity GetAGSGroupEntity(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups/{groupId}").Result;
            return HandleHttpResponse<AGSGroupEntity>(responseMessage, ResponseDataType.JObject);
        }

        public bool UpdateAGSGroupEntity(AGSGroupEntity groupEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(groupEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PutAsync($"groups/{groupEntity.Id}", data).Result;
            return HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public List<AGSFunctionClaimEntity> GetGroupFunctionClaims(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.GetAsync($"groups/{groupId}/functionclaims").Result;
            return HandleHttpResponse<List<AGSFunctionClaimEntity>>(responseMessage, ResponseDataType.JArray);
        }

        public bool DeleteAGSGroupEntity(string groupId)
        {
            using var client = GetAGSIdentityClient();
            var responseMessage = client.DeleteAsync($"groups/{groupId}").Result;
            return HandleHttpResponse<bool>(responseMessage, ResponseDataType.BooleanType);
        }

        public string AddAGSGroupEntity(AGSGroupEntity groupEntity)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(groupEntity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"groups", data).Result;
            return HandleHttpResponse<string>(responseMessage, ResponseDataType.PrimitiveType);
        }

        public bool ChangePassword(ChangeUserPasswordViewModel changeUserPasswordViewModel)
        {
            using var client = GetAGSIdentityClient();
            var json = JsonConvert.SerializeObject(changeUserPasswordViewModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = client.PostAsync($"users/{changeUserPasswordViewModel.UserId}/changepw", data).Result;
            return HandleHttpResponse<bool>(responseMessage, ResponseDataType.PrimitiveType);
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
