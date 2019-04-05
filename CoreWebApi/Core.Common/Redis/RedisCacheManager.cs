using Core.Common.Helper;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Redis
{
    /// <summary>
    /// Redis接口实现
    /// </summary>
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly string redisConnenctionString;

        public volatile ConnectionMultiplexer redisConnection;

        private readonly object redisConnectionLock = new object();
        private readonly string redisConfiguration;

        public RedisCacheManager(IOptions<AppSettings> options)
        {
            // 获取连接字符串
            redisConfiguration = options.Value.RedisCaching.ConnectionString;
            //string redisConfiguration = //AppsettingsHelper.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("Redis config is empty", nameof(redisConfiguration));
            }
            this.redisConnenctionString = redisConfiguration;
            this.redisConnection = GetRedisConnection();
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                // 反序列化
                return SerializeHelper.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Get(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                redisConnection.GetDatabase().StringSet(key, SerializeHelper.SerializeObject(value), cacheTime);
            }
        }

        /// <summary>
        /// 获取连接实例
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            // 已存在实例，直接返回
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            // 加锁，防止异步编程中出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    // 释放Redis连接
                    this.redisConnection.Dispose();
                }
                this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
            }
            return this.redisConnection;
        }

        #region 操作Redis常用方法
        #region Obj 操作

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T Get<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(database.StringGet(key, flags));
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(await database.StringGetAsync(key, flags));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T[] Get<T>(string[] keys, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
                redisKeys[i] = keys[i];
            var redisVals = database.StringGet(redisKeys, flags);
            var res = new T[redisVals.Length];
            for (int i = 0; i < redisVals.Length; i++)
                res[i] = SerializeHelper.DeserializeObject<T>(redisVals[i]);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<T[]> GetAsync<T>(string[] keys, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
                redisKeys[i] = keys[i];
            var redisVals = await database.StringGetAsync(redisKeys, flags);
            var res = new T[redisVals.Length];
            for (int i = 0; i < redisVals.Length; i++)
                res[i] = SerializeHelper.DeserializeObject<T>(redisVals[i]);
            return res;
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="timeSpan"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public bool Set<T>(string key, T value, int db = 0, TimeSpan? timeSpan = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.StringSet(key, SerializeHelper.SerializeObject(value), timeSpan, when, flags);
        }

        /// <summary>
        /// SetAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="timeSpan"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public Task<bool> SetAsync<T>(string key, T value, int db = 0, TimeSpan? timeSpan = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.StringSetAsync(key, SerializeHelper.SerializeObject(value), timeSpan, when, flags);
        }

        #endregion String 操作

        #region Hash
        /// <summary>
        /// HDel
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool HDel(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashDelete(key, hashid, flags);
        }

        public Task<bool> HDelAsync(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashDeleteAsync(key, hashid, flags);
        }

        /// <summary>
        /// HDel
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashids"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long HDel(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var keyArray = new RedisValue[hashids.Length];
            for (int i = 0; i < hashids.Length; i++)
            {
                keyArray[i] = hashids[i];
            }
            return database.HashDelete(key, keyArray, flags);
        }

        public Task<long> HDelAsync(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var keyArray = new RedisValue[hashids.Length];
            for (int i = 0; i < hashids.Length; i++)
            {
                keyArray[i] = hashids[i];
            }
            return database.HashDeleteAsync(key, keyArray, flags);
        }

        /// <summary>
        /// HExists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool HExists(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashExists(key, hashid, flags);
        }

        public Task<bool> HExistsAsync(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashExistsAsync(key, hashid, flags);
        }

        /// <summary>
        /// HGet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T HGet<T>(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(database.HashGet(key, hashid, flags));
        }

        public async Task<T> HGetAsync<T>(string key, string hashid, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(await database.HashGetAsync(key, hashid, flags));
        }

        /// <summary>
        /// HGet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashids"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public IDictionary<string, T> HGet<T>(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var hashFields = new RedisValue[hashids.Length];
            for (int i = 0; i < hashids.Length; i++)
            {
                hashFields[i] = hashids[i];
            }
            var redisValues = database.HashGet(key, hashFields, flags);
            var resDic = new Dictionary<string, T>();
            for (int i = 0; i < redisValues.Length; i++)
            {
                if (redisValues[i].HasValue)
                    resDic.Add(hashids[i], SerializeHelper.DeserializeObject<T>(redisValues[i]));
            }
            return resDic;
        }

        public async Task<IDictionary<string, T>> HGetAsync<T>(string key, string[] hashids, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var hashFields = new RedisValue[hashids.Length];
            for (int i = 0; i < hashids.Length; i++)
            {
                hashFields[i] = hashids[i];
            }
            var redisValues = await database.HashGetAsync(key, hashFields, flags);
            var resDic = new Dictionary<string, T>();
            for (int i = 0; i < redisValues.Length; i++)
            {
                if (redisValues[i].HasValue)
                    resDic.Add(hashids[i], SerializeHelper.DeserializeObject<T>(redisValues[i]));
            }
            return resDic;
        }

        /// <summary>
        /// HGetAll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public IDictionary<string, T> HGetAll<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            HashEntry[] hashEntry = database.HashGetAll(key, flags);
            var resDic = new Dictionary<string, T>();
            foreach (var item in hashEntry)
            {
                resDic.Add(item.Name, SerializeHelper.DeserializeObject<T>(item.Value));
            }
            return resDic;
        }

        public async Task<IDictionary<string, T>> HGetAllAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            HashEntry[] hashEntry = await database.HashGetAllAsync(key, flags);
            var resDic = new Dictionary<string, T>();
            foreach (var item in hashEntry)
            {
                resDic.Add(item.Name, SerializeHelper.DeserializeObject<T>(item.Value));
            }
            return resDic;
        }

        /// <summary>
        /// HIncrBy
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long HIncrBy(string key, string hashid, long value = 1, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashIncrement(key, hashid, value, flags);
        }

        /// <summary>
        /// HIncrByDouble
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public double HIncrByDouble(string key, string hashid, double value = 1.0, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashIncrement(key, hashid, value, flags);
        }

        /// <summary>
        /// HDecrBy
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long HDecrBy(string key, string hashid, long value = 1, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashDecrement(key, hashid, value, flags);
        }

        /// <summary>
        /// HDecrByDouble
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public double HDecrByDouble(string key, string hashid, double value = 1.0, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashDecrement(key, hashid, value, flags);
        }

        /// <summary>
        /// HKeys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public string[] HKeys(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var redisValues = database.HashKeys(key, flags);
            var res = new string[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                res[i] = redisValues[i];
            }
            return res;
        }

        /// <summary>
        /// HLen
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long HLen(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashLength(key, flags);
        }

        public Task<long> HLenAsync(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashLengthAsync(key, flags);
        }

        /// <summary>
        /// HSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashid"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool HSet<T>(string key, string hashid, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashSet(key, hashid, SerializeHelper.SerializeObject(value), when, flags);
        }

        public Task<bool> HSetAsync<T>(string key, string hashid, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.HashSetAsync(key, hashid, SerializeHelper.SerializeObject(value), when, flags);
        }

        /// <summary>
        /// HSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dic"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        public void HSet<T>(string key, IDictionary<string, T> dic, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var hashEntry = new HashEntry[dic.Count];
            int i = 0;
            foreach (var item in dic)
            {
                hashEntry[i++] = new HashEntry(item.Key, SerializeHelper.SerializeObject(item.Value));
            }
            database.HashSet(key, hashEntry, flags);
        }

        public Task HSetAsync<T>(string key, IDictionary<string, T> dic, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var hashEntry = new HashEntry[dic.Count];
            int i = 0;
            foreach (var item in dic)
            {
                hashEntry[i++] = new HashEntry(item.Key, SerializeHelper.SerializeObject(item.Value));
            }
            return database.HashSetAsync(key, hashEntry, flags);
        }

        /// <summary>
        /// HValues
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T[] HValues<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var redisValues = database.HashValues(key, flags);

            var res = new T[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                res[i] = SerializeHelper.DeserializeObject<T>(redisValues[i]);
            }
            return res;
        }

        public async Task<T[]> HValuesAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            var redisValues = await database.HashValuesAsync(key, flags);

            var res = new T[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                res[i] = SerializeHelper.DeserializeObject<T>(redisValues[i]);
            }
            return res;
        }

        /// <summary>
        /// HScan
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T[] HScan<T>(string key, string pattern, int db = 0)
        {
            //TODO 需要测试
            var database = redisConnection.GetDatabase(db);
            var values = database.HashScan(key, pattern);
            var res = new T[values.Count()];
            int i = 0;
            foreach (var item in values)
            {
                res[i++] = SerializeHelper.DeserializeObject<T>(item.Value);
            }
            return res;
        }

        #endregion

        #region Set
        /// <summary>
        /// SAdd
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        public void SAdd<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            database.SetAdd(key, SerializeHelper.SerializeObject(value), flags);
        }

        /// <summary>
        /// SAddAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> SAddAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetAddAsync(key, SerializeHelper.SerializeObject(value), flags);
        }
        /// <summary>
        /// Set元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        public long SCard(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetLength(key, flags);
        }

        public Task<long> SCardAsync(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetLengthAsync(key, flags);
        }

        /// <summary>
        /// 是否是Set成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool SIsMember<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetContains(key, SerializeHelper.SerializeObject(value));
        }

        public Task<bool> SIsMemberAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetContainsAsync(key, SerializeHelper.SerializeObject(value));
        }
        /// <summary>
        /// SPop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T SPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(database.SetPop(key, flags));
        }

        public async Task<T> SPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(await database.SetPopAsync(key, flags));
        }
        /// <summary>
        /// SRemove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool SRemove<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetRemove(key, SerializeHelper.SerializeObject(value), flags);
        }

        /// <summary>
        /// SRemoveAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> SRemoveAsync<T>(string key, T value, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.SetRemoveAsync(key, SerializeHelper.SerializeObject(value), flags);
        }

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
        public void LPush<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            database.ListLeftPush(key, SerializeHelper.SerializeObject(value), when, flags);
        }

        public Task LPushAsync<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.ListLeftPushAsync(key, SerializeHelper.SerializeObject(value), when, flags);
        }
        /// <summary>
        /// RPop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T RPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(database.ListRightPop(key, flags));
        }

        public async Task<T> RPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(await database.ListRightPopAsync(key, flags));
        }

        /// <summary>
        /// RPush
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public void RPush<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            database.ListRightPush(key, SerializeHelper.SerializeObject(value), when, flags);
        }

        public Task RPushAsync<T>(string key, T value, int db = 0, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return database.ListRightPushAsync(key, SerializeHelper.SerializeObject(value), when, flags);
        }

        /// <summary>
        /// LPop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T LPop<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(database.ListLeftPop(key, flags));
        }

        public async Task<T> LPopAsync<T>(string key, int db = 0, CommandFlags flags = CommandFlags.None)
        {
            var database = redisConnection.GetDatabase(db);
            return SerializeHelper.DeserializeObject<T>(await database.ListLeftPopAsync(key, flags));
        }

        #endregion

        #endregion
    }
}
