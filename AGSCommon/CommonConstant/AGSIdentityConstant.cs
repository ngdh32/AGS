using System;
namespace AGSCommon.CommonConstant
{
    public static class AGSIdentityConstant
    {
        public const string AGSAdminName = "admin";

        public const string FunctionClaimTypeConstant = "Function";

        public const string AGSClientIdConstant  = "AGS";

        public const string AGSIdentityScopeConstant  = "ags.identity";

        public const string AGSDocumentScopeConstant  = "ags.document";

        public const string AGSFunctionScopeConstant  = "roles";

        public const string AGSPolicyConstant = "ags_identity_auth_policy";
        public const string AGSAdminPolicyConstant = "ags_identity_auth_admin_policy";

        // Please make sure function claim properties end with "ClaimConstant"
        // for AGS Identity Function Claim menu, edit & read
        public const string AGSFunctionClaimMenuClaimConstant = "ags_identity_functionClaim_menu";
        public const string AGSFunctionClaimEditClaimConstant  = "ags_identity_functionClaim_edit";
        public const string AGSFunctionClaimReadClaimConstant  = "ags_identity_functionClaim_read";
        // for AGS Identity Group Admin menu, edit & read
        public const string AGSGroupMenuClaimConstant = "ags_identity_group_menu";
        public const string AGSGroupEditClaimConstant  = "ags_identity_group_edit";
        public const string AGSGroupReadClaimConstant  = "ags_identity_group_read";
        // for AGS Identity User Admin menu, edit & read
        public const string AGSUserMenuClaimConstant = "ags_identity_user_menu";
        public const string AGSUserEditClaimConstant  = "ags_identity_user_edit";
        public const string AGSUserReadClaimConstant  = "ags_identity_user_read";
        // for AGS Identity Config Admin menu, edit & read
        public const string AGSConfigMenuClaimConstant = "ags_identity_config_menu";
        public const string AGSConfigEditClaimConstant = "ags_identity_config_edit";
        public const string AGSConfigReadClaimConstant = "ags_identity_config_Read";

        public const string AGSUserDefaultPasswordConfigKey = "ags_identity_default_password";

        
    }
}
