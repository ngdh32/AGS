using System;
using AGSIdentity.Models.ExceptionModels;

namespace AGSIdentity.Services.ExceptionFactory
{
    public interface IExceptionFactory
    {
        AGSException GetErrorByCode(ErrorCodeEnum errorCode);
    }

    public enum ErrorCodeEnum
    {
        UsernameOrPasswordError = 1,
        UnknownError = 99999
    }
}
