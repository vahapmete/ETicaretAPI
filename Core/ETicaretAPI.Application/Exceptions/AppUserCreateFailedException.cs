using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class AppUserCreateFailedException : Exception
    {
        public AppUserCreateFailedException():base("Something went wrong!")
        {
        }

        public AppUserCreateFailedException(string? message) : base(message)
        {
        }

        public AppUserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AppUserCreateFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
