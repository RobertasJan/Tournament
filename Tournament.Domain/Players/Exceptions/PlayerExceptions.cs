using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Errors;

namespace Tournament.Domain.Players.Exceptions
{
    public class NoFirstnameException : APIException
    {
        public NoFirstnameException() : base(new ErrorCodeModel(10, "ERROR_MISSING_FIRSTNAME"))
        {
        }
    }
    public class NoLastnameException : APIException
    {
        public NoLastnameException() : base(new ErrorCodeModel(11, "ERROR_MISSING_LASTNAME"))
        {
        }
    }

    public class NoBirthdateException : APIException
    {
        public NoBirthdateException() : base(new ErrorCodeModel(12, "ERROR_MISSING_BIRTHDATE"))
        {
        }
    }

    public class NoPasswordException : APIException
    {
        public NoPasswordException() : base(new ErrorCodeModel(13, "ERROR_MISSING_PASSWORD"))
        {
        }
    }

    public class InvalidPasswordException : APIException
    {
        public InvalidPasswordException() : base(new ErrorCodeModel(14, "ERROR_INVALID_PASSWORD"))
        {
        }
    }
}
