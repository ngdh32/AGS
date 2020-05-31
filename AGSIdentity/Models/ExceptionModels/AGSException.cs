using System;
namespace AGSIdentity.Models.ExceptionModels
{
    public class AGSException : Exception
    {
        public int Code { get; set; }

        public AGSException(string message) : base(message)
        {
        }

        

    }
}
