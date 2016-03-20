using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using VKAnalyzer.DTO;
using System.Windows;


namespace VKAnalyzer
{

    public sealed class VkRepository : IRepository
    {
        /// <summary>
        /// Implementing singleton pattern
        /// </summary>
        static readonly VkRepository _instance = new VkRepository();
        public static VkRepository Instance
        {
            get
            {
                return _instance;
            }
        }
        public VkRepository()
        {
            AppID = "5294584";
            Scope = "groups,friends";
        }

        public string AccessToken { get; set; }
        public string LoggedInUserID { get; set; }

        public string AppID { get; set; }
        public string Scope { get; set; }
        public string RequestedUserID { get; set; }
        public bool SignedIn { get; set; }


        public List<string> GetGroups()
        {
            var t = Task.Factory.StartNew(() =>
                {
                    var searchResults = new List<string>();
                    var jsonString = "";
                    var webClient = new WebClient();

                    jsonString = webClient.DownloadString(string.Format("https://api.vk.com/method/{0}?user_id={1}&access_token={2}", "groups.get", VkRepository.Instance.RequestedUserID, VkRepository.Instance.AccessToken));

                    JObject res = JObject.Parse(jsonString);
                    IList<JToken> results = res["response"].Children().ToList(); // get JSON result objects into a list

                    foreach (JToken result1 in results)
                    {
                        string searchResult = JsonConvert.DeserializeObject<string>(result1.ToString());
                        searchResults.Add(searchResult.ToString());
                    }
                    return searchResults;
                });
            return t.Result;
        }


        internal List<Dictionary<string, string>> Grouped_groups()
        {
            var t = Task.Factory.StartNew(() =>
                {
                    string[] topics = File.ReadAllLines("../../Files/RESULT(formated_urls)T.csv");
                    List<Dictionary<string, string>> themes = new List<Dictionary<string, string>>();
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
                });
            return t.Result;
        }

        public List<User> GetFriends()
        {
            var t = Task.Factory.StartNew(() =>
                {
                    var webcl = new WebClient();
                    string jsonString = Encoding.UTF8.GetString(webcl.DownloadData(string.Format("https://api.vk.com/method/friends.get?user_id={0}&fields=nickname&order=name", VkRepository.Instance.LoggedInUserID)));
                    JObject res = JObject.Parse(jsonString);
                    IList<JToken> results = res["response"].Children().ToList();
                    List<User> searchResults = new List<User>();
                    foreach (JToken result1 in results)
                    {
                        var searchResult = JsonConvert.DeserializeObject<User>(result1.ToString());
                        searchResults.Add(searchResult);
                    }
                    return searchResults;
                });
            return t.Result;
        }


        public static List<string> is_in_file = new List<string>();



        public List<string> Compare_groups()
        {
            var t = Task.Factory.StartNew(() =>
                {
                    var users_gr = VkRepository.Instance.GetGroups();
                    var given_gr = VkRepository.Instance.Grouped_groups();
                    var out_list = new List<string>();
                    int total = 0;

                    foreach (Dictionary<string, string> topic in given_gr)
                    {
                        int counter = 0;
                        string e_name = "";

                        foreach (var elem in topic)
                        {
                            foreach (var gr in users_gr)
                                if (elem.Key == gr)
                                {
                                    counter = counter + 1;
                                    total++;
                                    is_in_file.Add(gr);
                                }
                            e_name = elem.Value;
                        }
                        string formated = string.Format("{0}: {1}", e_name, counter.ToString());
                        if (counter != 0)
                            out_list.Add(formated);
                    }
                    string total_formated = string.Format("\nTotal: {0}", total);
                    out_list.Add(total_formated);

                    return out_list;
                });
            return t.Result;
        }

        public List<KeyValuePair<string, int>> Compare_groups_stat()
        {
            var t = Task.Factory.StartNew(() =>
            {
                var users_gr = VkRepository.Instance.GetGroups();
                var given_gr = VkRepository.Instance.Grouped_groups();
                var out_list = new List<KeyValuePair<string, int>>();
                int total = 0;

                foreach (Dictionary<string, string> topic in given_gr)
                {
                    int counter = 0;
                    string e_name = "";

                    foreach (var elem in topic)
                    {
                        foreach (var gr in users_gr)
                            if (elem.Key == gr)
                            {
                                counter = counter + 1;
                                total++;
                                is_in_file.Add(gr);
                            }
                        e_name = elem.Value;
                    }

                    if (counter != 0)
                        out_list.Add(new KeyValuePair<string,int>(e_name, counter));
                }

                return out_list;
            });
            return t.Result;
        }

        public static double total = 0;
        public static double pluses = 0;
        public static double exclam = 0; // "big" groups, that are not in DATA file
        public static double small = 0;

        public List<string> UG_info() // UG -- user's groups
        {
            var t = Task.Factory.StartNew(() =>
                {
                    var UG_list = GetGroups();
                    List<int> searchResults = new List<int>();
                    var webClient = new WebClient();
                    var output = new Dictionary<string, int>();
                    List<string> list_output = new List<string>();

                    foreach (var elem in UG_list)
                    {
                        string link = string.Format("https://api.vk.com/method/{0}?group_id={1}", "groups.getMembers", elem);
                        JObject res = JObject.Parse(webClient.DownloadString(link));
                        JToken result = res["response"]["count"].ToString();
                        var num_result = int.Parse(result.ToString());
                        output.Add(elem, num_result);
                        searchResults.Add(num_result);
                    }
                    int num;
                    string str_output;
                    list_output.Add("          Id          -     Participants");

                    foreach (var elem in output)
                    {
                        num = 0;
                        total++;
                        foreach (var a in is_in_file)
                        {
                            if (elem.Key == a)
                            {
                                num = 1;
                                break;
                            }
                        }

                        if (num == 1)
                        {
                            str_output = String.Format("+ | {0}   -  {1}", elem.Key, elem.Value);
                            pluses++;
                        }
                        else
                        {
                            if (elem.Value > 10000)
                            {
                                str_output = String.Format("!  | {0}   -  {1}", elem.Key, elem.Value);
                                exclam++;
                            }
                            else
                            {
                                str_output = String.Format("   | {0}   -  {1}", elem.Key, elem.Value);
                                small++;
                            }

                        }
                        list_output.Add(str_output);
                    }

                    return list_output;
                });
            return t.Result;
        }


        internal static User GetUserInfo(string c)
        {
            WebClient web = new WebClient();
            string jsonString = Encoding.UTF8.GetString(web.DownloadData(string.Format("https://api.vk.com/method/users.get?user_ids={0}", c)));
            JObject res = JObject.Parse(jsonString);
            IList<JToken> results = res["response"].Children().ToList(); 
            return JsonConvert.DeserializeObject<User>(results[0].ToString());
        }
        internal static User GetUserInfo(string c, string fields)
        {
            WebClient web = new WebClient();
            string jsonString = Encoding.UTF8.GetString(web.DownloadData(string.Format("https://api.vk.com/method/users.get?user_ids={0}&fields={1}", c, fields)));
            JObject res = JObject.Parse(jsonString);
            IList<JToken> results = res["response"].Children().ToList();
            return JsonConvert.DeserializeObject<User>(results[0].ToString());
        }
        public static event Action <List<User>> UserDbLoaded;
        internal static void AddToDB(User user)
        {
            using (var c = new Context())
            {
                if (c.Users.Find(user.Uid) != null)
                {
                    c.Users.Add(user);
                    c.SaveChanges();
                }
                var query = c.Users.ToList();
                UserDbLoaded(query);
            }
        }
        public static int counter = 0;
        public static event Action ImageReady;
        internal static void DownloadFile (string path)
        {
            counter += 1;
            using (var web = new WebClient())
            {
                web.DownloadFileAsync(new Uri(path), string.Format("{0}avatar.jpg", counter.ToString()));
                web.DownloadFileCompleted += (sender, e) =>
                    {
                        ImageReady();
                    };
            }
        }

        
    }
}
