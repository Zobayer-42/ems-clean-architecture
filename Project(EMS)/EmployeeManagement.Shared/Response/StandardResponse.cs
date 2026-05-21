using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Shared.Response
{
    public class StandardResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<ErrorDetails> Errors { get; set; } = new();
    }
}
