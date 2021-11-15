using System;
using System.Collections.Generic;
using System.Linq;

namespace Challenge.Core.Response
{
    public class BaseResponse<T>
    {
        public List<ValidationError> ValidationErrors { get; set; } = new();
        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public void AddValidationError(string key, string value)
        {
            this.ValidationErrors.Add(new ValidationError { Key = key, Value = value });
        }

        public void SetMessage(string message)
        {
            if (this.ValidationErrors.Any())
                throw new Exception("SetMessage cannot be use with validaiton errors");

            this.HasError = false;
            this.Message = message;
        }

        public void SetErrorMessage(string message)
        {
            this.HasError = true;
            this.Message = message;
        }
    }
}