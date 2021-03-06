﻿using System;
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
        public AuthWindow()
        {
            InitializeComponent();
            AuthBrowser.Navigate(string.Format("https://oauth.vk.com/authorize?client_id={0}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope={1}&response_type=token&v=5.45", VkRepository.Instance.AppID, VkRepository.Instance.Scope));
        }

        public static event Action<string> OnLoggedIn;
        private void AuthBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.ToString().Contains("access_token") == true)
            {
                string url = e.Uri.ToString();
                char[] splitOptions = { '#', '&', '=' };
                VkRepository.Instance.AccessToken = url.Split(splitOptions)[2];
                VkRepository.Instance.LoggedInUserID = url.Split(splitOptions)[6];
                if (VkRepository.Instance.AccessToken != null)
                {
                    VkRepository.Instance.SignedIn = true;
                    if(OnLoggedIn != null)
                        OnLoggedIn(VkRepository.Instance.LoggedInUserID);
                    this.Close();
                }

            }
        }

        
    }
}
