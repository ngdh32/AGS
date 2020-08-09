using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.ViewModels.AGSIdentity;

namespace AGS.Services.AGSIdentity
{
    public interface IAGSIdentityService
    {
        Task<List<AGSFunctionClaimEntity>> GetFunctionClaimEntities();

        Task<List<AGSUserEntity>> GetAGSUserEntities();

        Task<List<AGSGroupEntity>> GetAGSGroupEntities();

        Task<AGSUserEntity> GetAGSUserEntity(string userId);

        Task<AGSGroupEntity> GetAGSGroupEntity(string groupId);

        Task<bool> UpdateAGSUserEntity(AGSUserEntity userEntity);

        Task<bool> UpdateAGSGroupEntity(AGSGroupEntity groupEntity);

        Task<List<AGSGroupEntity>> GetUserGroups(string userId);

        Task<List<AGSFunctionClaimEntity>> GetGroupFunctionClaims(string groupId);

        Task<bool> DeleteAGSUserEntity(string userId);

        Task<bool> DeleteAGSGroupEntity(string groupId);

        Task<string> AddAGSUserEntity(AGSUserEntity userEntity);

        Task<string> AddAGSGroupEntity(AGSGroupEntity groupEntity);

        Task<bool> ChangePassword(ChangeUserPasswordViewModel changeUserPasswordViewModel);

        Task<bool> ResetPassword(string userId);
    }
}
