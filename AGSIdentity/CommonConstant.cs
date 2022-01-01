using System;
namespace AGSIdentity
{
    public static class CommonConstant
    {
        public const string AGSAdminName = "admin";
        public const string FunctionClaimTypeConstant = "FunctionClaim";
        public const string AGSFunctionClaimResouceConstant = "FunctionClaimResource";
        public const string AGSClientIdConstant = "AGS";
        public const string AGSIdentityScopeConstant = "ags.identity";
        public const string AGSDocumentScopeConstant = "ags.document";

        
        // Please make sure function claim properties end with "ClaimConstant"
        // for AGS Identity Function Claim menu, edit & read
        public const string AGSFunctionClaimMenuClaimConstant = "ags_identity_functionClaim_menu";
        public const string AGSFunctionClaimEditClaimConstant = "ags_identity_functionClaim_edit";
        public const string AGSFunctionClaimReadClaimConstant = "ags_identity_functionClaim_read";
        // for AGS Identity Group Admin menu, edit & read
        public const string AGSGroupMenuClaimConstant = "ags_identity_group_menu";
        public const string AGSGroupEditClaimConstant = "ags_identity_group_edit";
        public const string AGSGroupReadClaimConstant = "ags_identity_group_read";
        // for AGS Identity User Admin menu, edit & read
        public const string AGSUserMenuClaimConstant = "ags_identity_user_menu";
        public const string AGSUserEditClaimConstant = "ags_identity_user_edit";
        public const string AGSUserReadClaimConstant = "ags_identity_user_read";
        // for AGS Identity Department menu, edit & read
        public const string AGSDepartmentMenuClaimConstant = "ags_identity_department_menu";
        public const string AGSDepartmentEditClaimConstant = "ags_identity_department_edit";
        public const string AGSDepartmentReadClaimConstant = "ags_identity_department_read";
        // for AGS Identity Changing password
        public const string AGSUserChangePasswordClaimConstant = "ags_identity_user_change_password";

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
