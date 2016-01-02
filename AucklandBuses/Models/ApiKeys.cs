using System.Collections.Generic;

namespace AucklandBuses.Models
{
    public class Key
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class ApiKeys
    {
        public List<Key> Keys { get; set; }
    }
}
