using System;
namespace AGSCommon
{
    public class CommonConstant
    {
        public static string FunctionClaimTypeConstant { get; private set; } = "Function";

        public static string AGSClientIdConstant { get; private set; } = "AGS";

        public static string AGSIdentityScopeConstant { get; private set; } = "ags.identity";

        public static string AGSDocumentScopeConstant { get; private set; } = "ags.document";

        public static string AGSFunctionScopeConstant {get; private set;} = "roles";
    }
}
