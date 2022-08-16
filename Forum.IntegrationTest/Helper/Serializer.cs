using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.Helper
{
    public static class Serializer
    {
        public static HttpContent SerializeForHttp(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
