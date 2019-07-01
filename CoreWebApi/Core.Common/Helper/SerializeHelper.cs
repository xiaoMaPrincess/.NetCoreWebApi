using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Helper
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeHelper
    {

        private static IsoDateTimeConverter _timeFormat;
        /// <summary>
        /// 序列化为字节数组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        /// <summary>
        /// 序列化为字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj, TimeFormat);
        }

        /// <summary>
        /// 反序列化 字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">字节数组</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] value)
        {
            if (value == null)
            {
                return default(T);
            }
            var jsonString = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 反序列化 字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);
            return JsonConvert.DeserializeObject<T>(obj);
        }

        /// <summary>
        /// 序列化时间格式
        /// </summary>
        public static IsoDateTimeConverter TimeFormat
        {
            get
            {
                if (_timeFormat == null)
                {
                    _timeFormat = new IsoDateTimeConverter()
                    {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                    };
                }
                return _timeFormat;
            }
            set
            {
                _timeFormat = value;
            }
        }
    }
}
