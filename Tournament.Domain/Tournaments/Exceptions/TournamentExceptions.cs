using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Errors;

namespace Tournament.Domain.Tournaments.Exceptions
{

    public class NoAddressException : APIException
    {
        public NoAddressException() : base(new ErrorCodeModel(19, "ERROR_MISSING_ADDRESS"))
        {
        }
    }

    public class NoDescriptionException : APIException
    {
        public NoDescriptionException() : base(new ErrorCodeModel(20, "ERROR_MISSING_DESCRIPTION"))
        {
        }
    }

    public class NoLongDescriptionException : APIException
    {
        public NoLongDescriptionException() : base(new ErrorCodeModel(21, "ERROR_MISSING_LONG_DESCRIPTION"))
        {
        }
    }

    public class InvalidDatesException : APIException
    {
        public InvalidDatesException() : base(new ErrorCodeModel(22, "ERROR_INVALID_DATES"))
        {
        }
    }

    public class DatesInThePastException : APIException
    {
        public DatesInThePastException() : base(new ErrorCodeModel(23, "ERROR_DATES_IN_THE_PAST"))
        {
        }
    }

    public class NoGroupsException : APIException
    {
        public NoGroupsException() : base(new ErrorCodeModel(24, "ERROR_MISSING_GROUPS"))
        {
        }
    }

    public class NoNameException : APIException
    {
        public NoNameException() : base(new ErrorCodeModel(25, "ERROR_MISSING_NAME"))
        {
        }
    }
}
