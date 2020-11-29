using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public class JsonSerializerJsonObject : JsonSerializer<IJsonObject>
    {
        public override IJsonObject Read(Type type, JToken json)
        {
            var value = (IJsonObject)Activator.CreateInstance(type);
            value.Read(json);
            return value;
        }

        public override JToken Write(IJsonObject value, Type type)
        {
            var json = new JObject();
            value.Write(json);
            return json;
        }

    }

}
