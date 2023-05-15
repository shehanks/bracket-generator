using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Country
    {
        [JsonIgnore]
        public int Id { get; internal set; }

        [JsonIgnore]
        public int GroupId { get; internal set; }

        [JsonProperty("group")]
        public string? Group { get; internal set; }

        [JsonProperty("seedNo")]
        public int? Seed { get; internal set; }

        [JsonProperty("name")]
        public string? Name { get; internal set; }
    }
}
