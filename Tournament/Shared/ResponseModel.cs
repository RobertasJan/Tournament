using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Errors;

namespace Tournament.Shared
{
    public class ResponseModel<T> : ResponseModel
    {
        public ResponseModel(T data, ErrorCodeModel errorModel) {
            this.Data = data;
            this.Error = errorModel;
        }
        public ResponseModel(T data)
        {
            this.Data = data;
        }

        public ResponseModel(ErrorCodeModel errorModel)
        {
            this.Error = errorModel;
        }
        public ResponseModel() { }
        public T? Data { get; set; }
    }

    public class ResponseModel 
    {
        public ErrorCodeModel? Error { get; set; }
    }

}
