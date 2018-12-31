using Core.Common.Helper;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

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
        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                // 反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
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
                redisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
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
            if(this.redisConnection!=null && this.redisConnection.IsConnected)
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
    }
}
