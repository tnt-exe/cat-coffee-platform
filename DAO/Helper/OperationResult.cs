using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Helper
{
    public class OperationResult<T>
    {
        public T? Payload { get; set; }
        public bool IsError { get; set; }
        public string? AccessToken { get; set; }
        public List<ErrorHelper> Errors { get; set; } = new List<ErrorHelper>();
        public void AddError(ErrorCode code, string message)
        {
            HandleError(code, message);
        }
        public void AddUnknownError(string message)
        {
            HandleError(ErrorCode.UnknownError, message);
        }

        public void ResetIsErrorFlag()
        {
            IsError = false;
        }

        private void HandleError(ErrorCode code, string message)
        {
            Errors.Add(new ErrorHelper { Code = code, Message = message });
            IsError = true;
        }

        public void AddValidationError(string foodIdAndSupplierIdCannotBeTheSame)
        {
            HandleError(ErrorCode.UnknownError, foodIdAndSupplierIdCannotBeTheSame);
        }
    }
}
