using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Domain.Errors
{
    //public static class ERRORS
    //{
    //    public static ErrorCodeModel ERROR_MISSING_FIRSTNAME = new ErrorCodeModel(10, "ERROR_MISSING_FIRSTNAME");
    //}

    public class ErrorCodeModel
    {
        public ErrorCodeModel(int errorCode, string errorMessage) {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int? ErrorCode { get; }
        public string? ErrorMessage { get; }
    }
}
