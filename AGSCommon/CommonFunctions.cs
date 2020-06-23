using System;
namespace AGSCommon
{
    public static class CommonFunctions
    {
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
