using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Redis
{
    public interface IRedisCacheManager
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        void Set(string key, object value, TimeSpan cacheTime);
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Get(string key);
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 清空
        /// </summary>
        void Clear();

        #region Obj
        T Get<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<T> GetAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        T[] Get<T>(string[] keys, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<T[]> GetAsync<T>(string[] keys, int db = 0, CommandFlags flags = CommandFlags.None);
        bool Set<T>(string key, T value, int db = 0, TimeSpan? timeSpan = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        Task<bool> SetAsync<T>(string key, T value, int db = 0, TimeSpan? timeSpan = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        #endregion

        #region Hash
        bool HDel(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<bool> HDelAsync(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        long HDel(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<long> HDelAsync(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None);
        bool HExists(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<bool> HExistsAsync(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        T HGet<T>(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<T> HGetAsync<T>(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None);
        IDictionary<string, T> HGet<T>(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<IDictionary<string, T>> HGetAsync<T>(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None);
        IDictionary<string, T> HGetAll<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<IDictionary<string, T>> HGetAllAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        long HIncrBy(string key, string hashid, long value = 1, int db = 0, CommandFlags flags = CommandFlags.None);
        double HIncrByDouble(string key, string hashid, double value = 1.0, int db = 0, CommandFlags flags = CommandFlags.None);
        long HDecrBy(string key, string hashid, long value = 1, int db = 0, CommandFlags flags = CommandFlags.None);
        double HDecrByDouble(string key, string hashid, double value = 1.0, int db = 0, CommandFlags flags = CommandFlags.None);
        string[] HKeys(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        long HLen(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<long> HLenAsync(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        bool HSet<T>(string key, string hashid, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);
        Task<bool> HSetAsync<T>(string key, string hashid, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);
        void HSet<T>(string key, IDictionary<string, T> dic, int db = 0, CommandFlags flags = CommandFlags.None);
        Task HSetAsync<T>(string key, IDictionary<string, T> dic, int db = 0, CommandFlags flags = CommandFlags.None);
        T[] HValues<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<T[]> HValuesAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        T[] HScan<T>(string key, string pattern, int db = 0);
        #endregion

        #region Set
        void SAdd<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<bool> SAddAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        long SCard(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<long> SCardAsync(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        bool SIsMember<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<bool> SIsMemberAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        T SPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<T> SPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);
        bool SRemove<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        Task<bool> SRemoveAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None);
        #endregion

        #region List

        /// <summary>
        /// LPush
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        void LPush<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);


        Task LPushAsync<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// RPop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        T RPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);


        Task<T> RPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// RPush
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        void RPush<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);

        Task RPushAsync<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// LPop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        T LPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);

        Task<T> LPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None);

        #endregion


    }
}
