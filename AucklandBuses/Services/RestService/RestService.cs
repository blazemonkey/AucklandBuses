using AucklandBuses.Models;
using AucklandBuses.Services.JsonService;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AucklandBuses.Services.RestService
{
    public class RestService : IRestService
    {
        [Dependency]
        public IJsonService JsonService { get; set; }

        public async Task<T> GetApi<T>(string apiUrl, string resourceUrl, List<KeyValuePair<string, string>> parameters = null)
        {
            var client = new HttpClient();
            string paramString = string.Empty;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    paramString = paramString + param.Key + "=" + param.Value + "&";
                }
            }

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(apiUrl + resourceUrl + "?" + paramString),
                Method = HttpMethod.Get,
            };
            var response = await client.SendRequestAsync(request);

            if (!response.IsSuccessStatusCode)
                return default(T);

            var contentString = await response.Content.ReadAsStringAsync();
            var content = JsonService.Deserialize<T>(contentString);

            return content;
        }
    }
}
