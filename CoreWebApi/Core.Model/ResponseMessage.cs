using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 返回消息体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseMessage<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
