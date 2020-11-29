using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giselle.Json
{
    public interface IJsonObject
    {
        void Read(JToken json);

        void Write(JToken json);
    }

}
