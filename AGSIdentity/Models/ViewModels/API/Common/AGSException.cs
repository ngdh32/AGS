using System;
using static AGSIdentity.Models.ViewModels.API.Common.AGSResponse;

namespace AGSIdentity.Models.ViewModels.API.Common
{
    public class AGSException : Exception
    {
        public ResponseCodeEnum responseCode { get; set; }

        public AGSException(): base()
        {
        }

        public AGSException(string message) : base(message)
        {
        }

        public AGSException(ResponseCodeEnum responseCode): base()
        {
            this.responseCode = responseCode;
        }
    }
}
