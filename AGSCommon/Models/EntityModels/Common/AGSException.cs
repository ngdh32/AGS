using System;
using static AGSCommon.Models.EntityModels.Common.AGSResponse;

namespace AGSCommon.Models.EntityModels.Common
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
