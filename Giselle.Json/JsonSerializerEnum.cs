using Giselle.Commons.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public class JsonSerializerEnum : JsonSerializer<Enum>
    {
        public override Enum Read(Type type, JToken json)
        {
            var name = json.Value<string>();
            return (Enum)EnumUtils.Cache(type).Parse(name);
        }

        public override JToken Write(Enum value, Type type)
        {
            return new JValue(value.ToString());
        }

    }

}
