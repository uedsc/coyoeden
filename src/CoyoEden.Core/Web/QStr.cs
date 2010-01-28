using System;
using Vivasky.Core.Infrastructure;
using System.Web;
using Vivasky.Core;
namespace CoyoEden.Core.Web
{
    /// <summary>
    /// Custom Query string helper.
    /// 1,Parsing d={x:'5',y:6} like querystring to a string dictionary automatically.
    /// </summary>
    public static class QStr
    {
        /// <summary>
        /// Convert the querystring of specified HttpRequest to a SerializableStringDictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static SerializableStringDictionary GetDic(this HttpRequest request) {
            var dic = new SerializableStringDictionary();
            var rawDic=request.QueryString;
            //fastest way to look through the dictionary.
            //http://dotnetperls.com/querystring-net
            if (rawDic.HasKeys()) {
                for (int i = 0; i < rawDic.Count; i++)
                {
                    dic.Add(rawDic.GetKey(i), rawDic.Get(i));
                }
            }
            return dic;
        }
        /// <summary>
        /// Get a query string's value by key,and convert it to the type T.
        /// Return default(T) if no such key or an exception accurs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpRequest request, string key) {
            return request.Get<T>(key, default(T));
        }
        /// <summary>
        /// Get a query string's value by key,and convert it to the type T.
        /// Return defaultVal if no such key or an exception accurs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpRequest request, string key,T defaultVal)
        {
            var dic = request.GetDic();
            if (!dic.ContainsKey(key)) return defaultVal;
            var obj=dic[key].As<T>();
            if (obj.IsDefault<T>()) return defaultVal;
            return obj;
        }
        /// <summary>
        /// get query string value of current request,and convert it to the type T.
        /// Return defaultVal if no such key or an exception accurs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static T GetQ<T>(string key, T defaultVal) {
            var r = HttpContext.Current.Request;
            return r.Get<T>(key, defaultVal);
        }
        /// <summary>
        /// get query string value of current request,and convert it to the type T.
        /// Return default(T) if no such key or an exception accurs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static T GetQ<T>(string key)
        {
            return GetQ<T>(key, default(T));
        }
    }
}
