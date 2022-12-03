using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
        }

        public ApiResult(bool isSuccessed, string message = null)
        {
            Message = message;
            isSuccessed = isSuccessed;
        }

        public ApiResult(bool isSuccessed, T data, string message = null)
        {
            Data = data;
            Message = message;
            isSuccessed = isSuccessed;
        }

        public string Message { get; set; }
        public bool IsSuccessed { get; set; }
        public T Data { get; }
    }
}
