using System;
namespace AGSDocumentCore.Models.DTOs.Services
{
    public class AGSUserResponse<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
