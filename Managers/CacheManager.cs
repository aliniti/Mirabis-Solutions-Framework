using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Miracle_Business_Solutions_Framework.Extensions;

namespace Miracle_Business_Solutions_Framework.Managers
{

    /// <summary>
    /// Static : because it's never instantiated.
    /// </summary>
    internal static class CacheManager
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        internal static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)Cache[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="expiry">the time for the object to expire</param>
        internal static void Add<T>(T objectToCache, string key, int expiry) where T : class
        {
            if (objectToCache != null) Cache.Add(key, objectToCache, DateTime.Now.AddMilliseconds(expiry));
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="expire">the time for the object to expire</param>
        internal static void Add(object objectToCache, string key, int expire)
        {
            if (objectToCache != null) Cache.Add(key, objectToCache, DateTime.Now.AddMilliseconds(expire));
        }
        
        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        internal static void Clear(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        internal static bool Exists(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAll()
        {
            return Cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }

        /// <summary>
        /// Below, we check if the cache contains the data that we are looking for based on the key.
        /// If it doesn't contain the data, we retrieve it from objects source and then add it to the cache.
        /// Then the next time that the method is called it won't have to hit the objects source, but simply
        /// get the data from memory. This saves a lot of time and overhead!
        ///
        /// BUT!!! We cannot use this method as each time the objectsToRetrieve is parsed to QueryCachedObjects it gives a copy of itself
        /// which then defeats the purpose of caching. by ref ??
        ///
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectsToRetrieve">Item(s) to be cached</param>
        /// <param name="cacheKey">Name of item(s)</param>
        /// <param name="expiry">the time for the object(s) to expire</param>
        /// <returns>the cached object(s)</returns>
        internal static T QueryCachedObjects<T>(T objectsToRetrieve, string cacheKey, int expiry) where T : class
        {
            // Check the cache
            var cachedObjects = Get<T>(cacheKey);

            if (cachedObjects == null)
            {
                // Go and retrieve the object(s)
                cachedObjects = objectsToRetrieve;

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                Add(cachedObjects, cacheKey, expiry);
                Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            else
            {
                Logger.DebugLog("[Cache] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }

            return cachedObjects;
        }

    }
}