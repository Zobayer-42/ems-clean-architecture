using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Shared.Response
{
    public static class ResponseHelper
    {
        public static StandardResponse<T> Success<T>(T data, string message = "Success", string statusCode = "200")
        => new()
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = new()
        };

        public static StandardResponse<T> Fail<T>(string message, string statusCode = "400", List<ErrorDetails>? errors = null)
            => new()
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Data = default,
                Errors = errors ?? new()
            };

        public static StandardResponse<T> ValidationFail<T>(List<ErrorDetails> errors, string message = "Validation failed")
            => new()
            {
                IsSuccess = false,
                StatusCode = "422",
                Message = message,
                Data = default,
                Errors = errors
            };
    }
}
