using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public interface IJsonSerializer
    {
        object Read(Type type, JToken json);

        JToken Write(object value, Type type);

        bool CanConvert(Type type);

    }

    public abstract class JsonSerializer<T> : IJsonSerializer
    {
         object IJsonSerializer.Read(Type type, JToken json)
        {
            return this.Read(type, json);
        }

        JToken IJsonSerializer.Write(object value, Type type)
        {
            if (value is T t)
            {
                return this.Write(t, type);
            }
            else
            {
                return null;
            }

        }

        public bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public abstract T Read(Type type, JToken json);

        public abstract JToken Write(T value, Type type);

    }

}
