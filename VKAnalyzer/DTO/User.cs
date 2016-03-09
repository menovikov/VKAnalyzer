using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAnalyzer.DTO
{
    public class User
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public override string ToString()
        {
            return string.Format(FirstName + " " + LastName);
        }

    }
}
