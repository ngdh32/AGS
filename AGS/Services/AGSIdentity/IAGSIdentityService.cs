using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.ViewModels.AGSIdentity;

namespace AGS.Services.AGSIdentity
{
    public interface IAGSIdentityService
    {
        List<AGSFunctionClaimEntity> GetFunctionClaimEntities();

        List<AGSUserEntity> GetAGSUserEntities();

        List<AGSGroupEntity> GetAGSGroupEntities();

        AGSUserEntity GetAGSUserEntity(string userId);

        AGSGroupEntity GetAGSGroupEntity(string groupId);

        bool UpdateAGSUserEntity(AGSUserEntity userEntity);

        bool UpdateAGSGroupEntity(AGSGroupEntity groupEntity);

        List<AGSGroupEntity> GetUserGroups(string userId);

        List<AGSFunctionClaimEntity> GetGroupFunctionClaims(string groupId);

        bool DeleteAGSUserEntity(string userId);

        bool DeleteAGSGroupEntity(string groupId);

        string AddAGSUserEntity(AGSUserEntity userEntity);

        string AddAGSGroupEntity(AGSGroupEntity groupEntity);

        bool ChangePassword(ChangeUserPasswordViewModel changeUserPasswordViewModel);
    }
}
