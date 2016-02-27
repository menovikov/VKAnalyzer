using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace VKAnalyzer
{

    public class Repository
    {
        //https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
        string requestText = "https://api.vk.com/method/{0}?{1}&access_token={2}";
        public static string client_id = "5294584";
        public static string scope = "groups";
        string access_token = AuthWindow.access_token;
        //string requested_user_id = MainWindow.requestedUserId.ToString();
        string user_id = AuthWindow.user_id;

        Dictionary<string, string> APImethods = new Dictionary<string, string>()
        {
            
        };

        public static List<string> GetGroups()
        {
            var webClient = new WebClient();
            string jsonString = webClient.DownloadString(Query("groups.get", MainWindow.requestedUserId.ToString() , AuthWindow.access_token));
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

        internal static List<Dictionary<string, string>> Groped_groups()
        {
            string[] topics = File.ReadAllLines("../../Files/RESULT(formated_urls)T.csv");
            List<Dictionary<string, string>> themes = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>();
            foreach (var topic in topics)
            {
                Dictionary<string, string> dthemes = new Dictionary<string, string>();
                string[] lol = topic.Split(';');
                foreach (var elem in lol)
                {
                    if ((elem != "-1") & (elem != lol[0]))
                        dthemes.Add(elem, lol[0]);
                }
                themes.Add(dthemes);
            }
            return themes;
        }

        static string Query(string method, string parameters, string accesstoken)
        {
            return string.Format("https://api.vk.com/method/{0}?user_id={1}&access_token={2}", method, parameters, accesstoken);
        }

        public static List<string> Compare_groups()
        {
            var users_gr = Repository.GetGroups();
            var given_gr = Repository.Groped_groups();
            // var out_dict = new Dictionary<int, string>();
            var out_list = new List<string>();
            int total = 0;

            foreach (Dictionary<string, string> topic in given_gr)
            {
                int counter = 0;
                string e_name = "";
                foreach (var elem in topic)
                {

                    foreach (var gr in users_gr)
                    {
                        if (elem.Key == gr)
                        {
                            counter = counter + 1;
                            total++;
                        }
                    }
                    e_name = elem.Value;
                }
                string formated = string.Format("{0}: {1}", e_name, counter.ToString());
                out_list.Add(formated);
            }
            string total_formated = string.Format("Total: {0}", total);
            out_list.Add(total_formated);

            return out_list;
        }
    }
}