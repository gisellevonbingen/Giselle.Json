using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public static class JsonUtils
    {
        public static JsonSerializerRegistry Registry { get; }

        static JsonUtils()
        {
            Registry = new JsonSerializerRegistry();
            Registry.Register(new JsonSerializerEnum());
            Registry.Register(new JsonSerializerJsonObject());
            Registry.Register(new JsonSerializerRandom());
            Registry.Register(new JsonSerializerGuid());
        }

        public static bool IsNull(this JToken json) => json == null || json.Type == JTokenType.Null;

        public static JToken Dictionary<K, V>(this Dictionary<K, V> value, JToken token, Func<K, string> keyFunc, Func<V, JToken> valueFunc)
        {
            foreach (var pair in value)
            {
                var pKey = keyFunc(pair.Key);
                var pValue = valueFunc(pair.Value);
                token[pKey] = pValue;
            }

            return token;
        }

        public static JToken Dictionary<K, V>(this Dictionary<K, V> value, Func<K, string> keyFunc, Func<V, JToken> valueFunc)
        {
            var json = new JObject();
            return value.Dictionary(json, keyFunc, valueFunc);
        }

        public static Dictionary<K, V> Dictionary<K, V>(this JToken value, Func<string, K> keyFunc, Func<JToken, V> valueFunc)
        {
            var map = new Dictionary<K, V>();

            if (value is JObject jObject)
            {
                foreach (var pair in jObject)
                {
                    var pkey = keyFunc(pair.Key);
                    var pValue = valueFunc(pair.Value);

                    map[pkey] = pValue;
                }

            }

            return map;
        }

        public static bool TryGet(this JToken value, string propertyName, out JToken token)
        {
            if (value is JObject jObject)
            {
                if (jObject.TryGetValue(propertyName, out var jToken) == true)
                {
                    token = jToken;
                    return true;
                }

            }

            token = null;
            return false;
        }

        public static T IfExist<T>(this JToken value, string propertyName, Func<JToken, T> func, T fallback = default)
        {
            if (TryGet(value, propertyName, out var token) == true)
            {
                return token.IfExist(func);
            }

            return fallback;
        }

        public static T IfExist<T>(this JToken value, Func<JToken, T> func, T fallback = default)
        {
            if (value == null)
            {
                return fallback;
            }
            else if (value is JValue jValue)
            {
                if (jValue.Value == null)
                {
                    return fallback;
                }

            }

            return func(value);
        }


        public static T[] Array<T>(this JToken value, string propertyName)
        {
            return value.IfExist(propertyName, t => Array<T>(t)) ?? new T[0];
        }

        public static T[] Array<T>(this JToken value)
        {
            if (value is JArray jArray)
            {
                return jArray.Values<T>().ToArray();
            }
            else if (value != null)
            {
                return new T[] { value.Value<T>() };
            }
            else
            {
                return new T[0];
            }

        }

        public static T[] Array<T>(this JToken value, Func<JToken, T> func)
        {
            return value.Array<JToken>().Select(t => func(t)).ToArray();
        }

        public static JToken Array<T>(this IEnumerable<T> value, Func<T, JToken> func)
        {
            return new JArray(value.Select(e => func(e)));
        }

        public static JObject Object(this JToken value)
        {
            return (value as JObject) ?? new JObject();
        }

        public static JObject Object(this JToken value, string propertyName)
        {
            return value.IfExist(propertyName, t => t as JObject) ?? new JObject();
        }

        public static T Read<T>(this JToken json, string propertyName) where T : new()
        {
            var value = json.Value<JToken>(propertyName);
            return value.Read<T>();
        }

        public static T Read<T>(JToken json, Type type)
        {
            var serializer = Registry.Find(type);
            var value = serializer.Read(type, json);
            return (T)value;
        }

        public static T Read<T>(this JToken json)
        {
            if (json.IsNull() == true)
            {
                return default;
            }

            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType == null)
            {
                return Read<T>(json, type);
            }
            else
            {
                return Read<T>(json, underlyingType);
            }

        }

        public static JToken Write<T>(this T value, Type type)
        {
            var serializer = Registry.Find(type);
            var json = serializer.Write(value, type);
            return json;
        }

        public static JToken Write<T>(this T value)
        {
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType == null)
            {
                return Write(value, type);
            }
            else
            {
                return Write(value, underlyingType);
            }

        }

    }

}
