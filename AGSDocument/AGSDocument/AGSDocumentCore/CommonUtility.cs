using System;
namespace AGSDocumentCore
{
    public static class CommonUtility
    {
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
