using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Common.Http.Extensions
{
    public static class HttpContentExtensions
    {

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var str = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(str);
        }

    }
}
