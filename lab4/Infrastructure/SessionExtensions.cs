using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace lab4.Infrastructure
{
    // Методы расширения для ISession
    public static class SessionExtensions
    {
        //Запись параметризованного объекта  в сессию
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        //Считывание параметризованного объекта из сессии
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }


    }
}