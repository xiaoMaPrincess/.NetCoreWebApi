using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 树形数据接口
    /// </summary>
    public interface ITreeData
    {
        /// <summary>
        /// 父节点ID
        /// </summary>
        Guid? ParentID { get; set; }
    }
    /// <summary>
    /// 树形数据泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeData<T> : ITreeData where T : BaseModel
    {
        /// <summary>
        /// 实现这个方法获得所有的子节点数据
        /// </summary>
        /// <returns>所有子节点数据</returns>
        List<T> GetChildrenList { get; }

        /// <summary>
        /// 获取父节点
        /// </summary>
        T Parent { get; }
    }
}
