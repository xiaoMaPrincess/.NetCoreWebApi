using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 通用返回信息类（API）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> Data { get; set; }
    }
}
