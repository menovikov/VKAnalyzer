using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace VKAnalyzer
{

    public class Repository
    {
        //https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
        string requestText = "https://api.vk.com/method/{0}?{1}&access_token={2}";
        public static string client_id = "5294584";
        public static string scope = "groups";
        string access_token = AuthWindow.access_token;
        
        string user_id = AuthWindow.user_id;

        Dictionary<string, string> APImethods = new Dictionary<string, string>()
        {
            
        };

        public static List<string> GetGroups()
        {
            var webClient = new WebClient();
            string jsonString = webClient.DownloadString(Query("groups.get", AuthWindow.user_id, AuthWindow.access_token));
            JObject res = JObject.Parse(jsonString);
            IList<JToken> results = res["response"].Children().ToList(); // get JSON result objects into a list
            List<string> searchResults = new List<string>();
            foreach (JToken result1 in results)
            {
                string searchResult = JsonConvert.DeserializeObject<string>(result1.ToString());
                searchResults.Add(searchResult.ToString());
            }
            return searchResults;
        }

        static string Query(string method, string parameters, string accesstoken)
        {
            return string.Format("https://api.vk.com/method/{0}?{1}&access_token={2}", method, parameters, accesstoken);
        }


    }
}