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

        [JsonProperty("bdate")]
        public string Bdate { get; set; }

        [JsonProperty("followers_count")]
        public string Followers { get; set; }

        private string _gender;
        [JsonProperty("sex")]
        public string Gender 
        {
            get
            {
                return _gender;
            }
            set
            {
                if (value == "0")
                    _gender = "M";
                else if (value == "1")
                    _gender = "F";
            }
        }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }
        
       
        
        [JsonProperty("photo_200")]
        public string Photo { get; set; }

        public override string ToString()
        {
            return string.Format(FirstName + " " + LastName);
        }

    }
}
