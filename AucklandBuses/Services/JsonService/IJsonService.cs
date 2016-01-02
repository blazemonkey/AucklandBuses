using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Services.JsonService
{
    public interface IJsonService
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
    }
}
