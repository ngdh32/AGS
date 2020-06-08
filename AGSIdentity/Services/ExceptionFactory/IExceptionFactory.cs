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
        UsernameOrPasswordError = 90000,
        RedirectUrlError = 90001,
        UnknownError = 99999
    }
}
