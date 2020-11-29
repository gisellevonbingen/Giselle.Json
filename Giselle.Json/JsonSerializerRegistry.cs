using Giselle.Commons.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public class JsonSerializerRegistry : ReadOnlyList<IJsonSerializer>
    {
        public JsonSerializerRegistry()
        {

        }

        public IJsonSerializer Find(Type type)
        {
            return this.FirstOrDefault(s => s.CanConvert(type));
        }

        public void Register<T>(JsonSerializer<T> serializer)
        {
            this.List.Add(serializer);
        }

    }

}
