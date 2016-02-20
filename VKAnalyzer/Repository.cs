using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;


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

        public static string GetGroups()
        {
            
            var webClient = new WebClient();
            string result = webClient.DownloadString(Query("groups.get", "50259434", AuthWindow.access_token));
            return result;//JsonConvert.DeserializeObject<List<string>>(result);       
        }

        static string Query(string method, string parameters, string accesstoken)
        {
            return string.Format("https://api.vk.com/method/{0}?{1}&access_token={2}", method, parameters, accesstoken);
        }
    }
}