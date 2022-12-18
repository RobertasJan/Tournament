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

    public class NoEmailException : APIException
    {
        public NoEmailException() : base(new ErrorCodeModel(15, "ERROR_MISSING_EMAIL"))
        {
        }
    }

    public class NoConfirmPasswordException : APIException
    {
        public NoConfirmPasswordException() : base(new ErrorCodeModel(16, "ERROR_MISSING_CONFIRM_PASSWORD"))
        {
        }
    }

    public class PasswordsDoNotMatchException : APIException
    {
        public PasswordsDoNotMatchException() : base(new ErrorCodeModel(17, "ERROR_PASSWORDS_DO_NOT_MATCH"))
        {
        }
    }

    public class BirthDateInTheFutureException : APIException
    {
        public BirthDateInTheFutureException() : base(new ErrorCodeModel(18, "ERROR_BIRTH_DATE_IN_THE_FUTURE"))
        {
        }
    }
}
