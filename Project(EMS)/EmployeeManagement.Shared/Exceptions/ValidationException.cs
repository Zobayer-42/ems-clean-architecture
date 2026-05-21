using EmployeeManagement.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Shared.Exceptions
{
    public class ValidationException : Exception
    {
        public List<ErrorDetails> Errors { get; }

        public ValidationException(List<ErrorDetails> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
