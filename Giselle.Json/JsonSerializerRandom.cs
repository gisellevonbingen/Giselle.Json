using Giselle.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public class JsonSerializerRandom : JsonSerializer<Random>
    {
        public override Random Read(Type type, JToken json)
        {
            var bytes = json.Array<byte>();
            return RandomUtils.Read(bytes);
        }

        public override JToken Write(Random value, Type type)
        {
            var bytes = RandomUtils.Save(value);
            return new JArray(bytes);
        }

    }

}
