using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public class JsonSerializerGuid : JsonSerializer<Guid>
    {
        public override Guid Read(Type type, JToken json)
        {
            var bytes = json.Array<byte>();
            return new Guid(bytes);
        }

        public override JToken Write(Guid value, Type type)
        {
            var bytes = value.ToByteArray();
            return bytes.Array(b => b);
        }

    }

}
