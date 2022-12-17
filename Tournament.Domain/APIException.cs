using Tournament.Domain.Errors;

namespace Tournament.Domain
{
    public abstract class APIException : Exception
    {
        public ErrorCodeModel ErrorCodeModel { get; }
        public APIException(ErrorCodeModel errorCodeModel) : base(errorCodeModel.ErrorMessage)
        {
            this.ErrorCodeModel = errorCodeModel;
        }
    }
}
