using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator.Models
{
    public class Participants
    {
        [JsonProperty("country")]
        public IList<Country> Countries { get; set; } = new List<Country>();

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var isGroupTournament = Countries.First().Group != null;
            var allGroups = Countries.Select(c => c.Group).Distinct();
            var groupMapper = new Dictionary<string, int>();

            for (int i = 0; i < allGroups.Count(); i++)
            {
                var el = allGroups.ElementAt(i);
                if (el != null)
                    groupMapper.Add(el, i + 1);
            }

            var counter = 1;
            foreach (var country in Countries)
            {
                country.Id = counter;

                if (!isGroupTournament)
                    country.GroupId = 1;
                else
                {
                    if (country.Group != null)
                        country.GroupId = groupMapper[country.Group];
                }

                ++counter;
            }
        }
    }
}
