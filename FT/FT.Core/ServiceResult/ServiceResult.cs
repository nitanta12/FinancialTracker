using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FT.Core.ServiceResult
{
    public class ServiceResult : IServiceResult
    {
        public string MessageType { get; set; }

        public List<string> Message { get; set; }

        public bool Status { get; set; }

        [JsonConstructor]
        public ServiceResult(bool status, List<string> message = null, string messageType = null)
        {
            if (string.IsNullOrEmpty(messageType))
                MessageType = Core.ServiceResult.MessageType.Success;
            else
                MessageType = messageType;
            if (message == null)
                message = new List<string>();
            Message = message;
            Status = status;
        }

        public ServiceResult(bool status, List<string> message)
        {
            Status = status;
            Message = message;
        }
        public ServiceResult<T> ToResult<T>()
        {
            return new ServiceResult<T>(Status, Message, MessageType);
        }
        public static ServiceResult Fail(string message)
        {
            return new ServiceResult(false) { Message = new List<string> { message }, MessageType = Core.ServiceResult.MessageType.Warning };
        }

        public static ServiceResult Fail(string message, string messageType)
        {
            return new ServiceResult(false) { MessageType = messageType, Message = new List<string> { message } };
        }

        public static ServiceResult Fail(List<string> messages)
        {
            return new ServiceResult(false) { Message = messages, MessageType = Core.ServiceResult.MessageType.Warning };
        }

        public static ServiceResult Fail(List<string> messages, string messageType)
        {
            return new ServiceResult(false) { MessageType = messageType, Message = messages };
        }

        public static ServiceResult Success(string message)
        {
            return new ServiceResult(true) { MessageType = Core.ServiceResult.MessageType.Success, Message = [message] };
        }
        public static ServiceResult Success(List<string> messages)
        {
            return new ServiceResult(true) { MessageType = Core.ServiceResult.MessageType.Success, Message = messages };
        }
        public ServiceResult()
        {

        }
    }
}

namespace FT.Core.ServiceResult
{
    public class ServiceResult<T> : ServiceResult
    {

        private T ResposeData { get; set; }
        public T Data
        {
            get => ResposeData;
            set => ResposeData = value;
        }

        public ServiceResult ToResult()
        {
            return new ServiceResult(Status, Message, MessageType);
        }
        public ServiceResult<T> ToResult<T>()
        {
            return new ServiceResult<T>(Status, Message, MessageType);
        }

        public ServiceResult(bool status, List<string> message = null, string messageType = null)
            : base(status, message, messageType)
        {
        }

        public new static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T>(false, [message], Core.ServiceResult.MessageType.Warning);
        }

        public static ServiceResult<T> Fail(T data, string message)
        {
            return new ServiceResult<T>(false) { Data = data, Message = [message], MessageType = Core.ServiceResult.MessageType.Warning };
        }

        public new static ServiceResult<T> Fail(string message, string messageType)
        {
            return new ServiceResult<T>(false) { MessageType = messageType, Message = new List<string> { message } };
        }
        public new static ServiceResult<T> Fail(List<string> messages)
        {
            return new ServiceResult<T>(false) { Message = messages, MessageType = Core.ServiceResult.MessageType.Warning };
        }
        public new static ServiceResult<T> Fail(List<string> messages, string messageType)
        {
            return new ServiceResult<T>(false) { MessageType = messageType, Message = messages };
        }

        public new static ServiceResult<T> Success(string message)
        {
            return new ServiceResult<T>(true) { Message = new List<string> { message }, MessageType = Core.ServiceResult.MessageType.Success };
        }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>(true) { Data = data, MessageType = Core.ServiceResult.MessageType.Success };
        }

        public static ServiceResult<T> Success(T data, string message)
        {
            return new ServiceResult<T>(true) { Data = data, Message = new List<string> { message }, MessageType = Core.ServiceResult.MessageType.Success };
        }

        public static ServiceResult<T> Success(T data, List<string> messages)
        {
            return new ServiceResult<T>(true) { Data = data, Message = messages, MessageType = Core.ServiceResult.MessageType.Success };
        }
    }
}
