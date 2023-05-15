using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Events
    {
        [JsonProperty("winners")]
        public List<List<string>> Winners { get; set; } = new List<List<string>>();

        [JsonProperty("champion")]
        public string? Champion { get; set; }
    }
}
