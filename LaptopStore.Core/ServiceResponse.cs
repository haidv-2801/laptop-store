using LaptopStore.Core.Enums;
using LaptopStore.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core
{
    public class ServiceResponse
    {

        public ResponseCode Code { get; set; } = ResponseCode.Success;

        public string Message { get; set; } = string.Empty;

        public string UserMessage { get; set; } = string.Empty;

        public object Data { get; set; }

        public bool Success { get; set; }

        public string Logs { get; set; }

        public ServiceResponse OnError(Exception ex, ResponseCode code = ResponseCode.BusinessError)
        {
            this.Message = ex.Message;
            this.Code = code;
            this.Success = false;
            this.Logs = AsyncLocalLogger.EndRequestLogAndSerialize();
            return this;
        }

        public ServiceResponse ResponseData(string message, object? data, bool success = true, ResponseCode code = ResponseCode.BusinessError)
        {
            this.Message = message;
            this.Code = code;
            this.Data = data;
            this.Success = success;
            this.Logs = AsyncLocalLogger.EndRequestLogAndSerialize();
            return this;
        }

        public ServiceResponse OnSuccess(object data)
        {
            this.Code = ResponseCode.Success;
            this.Message = "Thành công";
            this.Data = data;
            this.Success = true;
            this.Logs = AsyncLocalLogger.EndRequestLogAndSerialize();
            return this;
        }
    }
}
