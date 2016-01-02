using AucklandBuses.Models;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AucklandBuses.Services.WebClientService
{
    public interface IWebClientService
    {
        Task<long> GetFileSize(string url);
        Task<IEnumerable<T>> DownloadFile<T,TMap>(string url) where T : ITransit, new()
                                                              where TMap : CsvClassMap;
    }
}
