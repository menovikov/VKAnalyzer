using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VKAnalyzer
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public static string user_id = "";
        //public static string access_token = "";
        public static string access_token { get; set; }
        
        public AuthWindow()
        {
            InitializeComponent();
            AuthBrowser.Navigate(string.Format("https://oauth.vk.com/authorize?client_id={0}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope={1}&response_type=token&v=5.45", Repository.client_id, Repository.scope));
        }

        private void AuthBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.ToString().Contains("access_token") == true)
            {
                //http://REDIRECT_URI#access_token= 533bacf01e11f55b536a565b57531ad114461ae8736d6506a3&expires_in=86400&user_id=8492
                string url = e.Uri.ToString();
                char[] splitOptions = { '#', '&', '=' };
                access_token = url.Split(splitOptions)[2];
                user_id = url.Split(splitOptions)[6];
                if (access_token != null)
                    this.Close();
            }
        }

        
    }
}
