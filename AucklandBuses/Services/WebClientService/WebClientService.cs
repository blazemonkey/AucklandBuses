using AucklandBuses.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace AucklandBuses.Services.WebClientService
{
    public class WebClientService : IWebClientService
    {
        public async Task<long> GetFileSize(string url)
        {
            var req = WebRequest.Create(url);
            req.Method = "HEAD";
            var resp = await req.GetResponseAsync();
            return resp.ContentLength;
        }

        public async Task<IEnumerable<T>> DownloadFile<T, TMap>(string url) where T : ITransit, new()
                                                                            where TMap : CsvClassMap
        {
            try
            {
                var items = new List<T>();

                var req = WebRequest.Create(url);
                var resp = await req.GetResponseAsync();
                var stream = resp.GetResponseStream();                               

                var reader = new StreamReader(stream);
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<TMap>();

                var records = csv.GetRecords(typeof(T));
                foreach (var r in records)
                {
                    items.Add((T)r);
                }

                return items;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
