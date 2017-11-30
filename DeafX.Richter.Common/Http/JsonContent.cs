using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DeafX.Richter.Common.Http
{
    public class JsonContent : StringContent
    {

        public JsonContent(object jsonObject)
            : this(jsonObject, Encoding.UTF8) { }

        public JsonContent(object jsonObject, Encoding encoding)
            : base(JsonConvert.SerializeObject(jsonObject, Formatting.None), encoding, "application/json")
        {
            
        }

    }
}
