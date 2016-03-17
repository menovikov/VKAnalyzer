﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VKAnalyzer.DTO;

namespace VKAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //r1 and r2 are provided due to
        //inability to use factory pattern
        //with sungleton
        private VkRepository r1 = VkRepository.Instance;
        private FacebookRepository r2 = FacebookRepository.Instance;


        public MainWindow()
        {
            InitializeComponent();
            VkRepository.ImageReady += () => 
            {
                UserAvatar.Children.Clear();

                Image i = new Image();
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(string.Format("{0}avatar.jpg", VkRepository.counter.ToString()), UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                i.Source = src;
                i.Stretch = Stretch.Uniform;
                UserAvatar.Children.Add(i);
            };
            AuthWindow.OnLoggedIn += (c) =>
                {
                    AuthInfo.Text = VkRepository.GetUserInfo(c).ToString();
                };
        }

        private void Auth()
        {
            AuthWindow window = new AuthWindow();
            window.Show();
        }

        private void CompareGroupsButton_Click(object sender, RoutedEventArgs e)
        {

            if (!VkOn.IsEnabled && r1.SignedIn == true)
            {
                if (UserIdTextBlock.Text != "")
                {
                    UserAvatar.Children.Clear();
                    UserInfoListView.Items.Clear();
                    User u = VkRepository.GetUserInfo(UserIdTextBlock.Text, "bdate,sex,followers_count");

                    UserInfoListView.Items.Add("Name: " + u);
                    UserInfoListView.Items.Add("Birth date: " + u.Bdate);
                    UserInfoListView.Items.Add("Gender: " + u.Gender);
                    UserInfoListView.Items.Add("Followers: " + u.Followers);
                    //
                    VkRepository.total = 0;
                    VkRepository.pluses = 0;
                    VkRepository.small = 0;
                    VkRepository.exclam = 0;
                    //
                    VkRepository.Instance.RequestedUserID = UserIdTextBlock.Text;
                    ListBox.ItemsSource = r1.Compare_groups();
                    ListBox.ItemsSource = VkRepository.Instance.Compare_groups();
                    ListBox1.ItemsSource = VkRepository.Instance.UG_info();

                    // statistics below
                    Statistics.Items.Clear();
                    Statistics.Items.Add(String.Format("Total: {0} groups", VkRepository.total));
                    double match_percent = VkRepository.pluses / VkRepository.total * 100;
                    Statistics.Items.Add(String.Format("{0} groups matched ({1:0.#}%)", VkRepository.pluses, match_percent));

                    double match_percent1 = VkRepository.small / VkRepository.total * 100;
                    Statistics.Items.Add(String.Format("{0} small groups ({1:0.#}%)", VkRepository.small, match_percent1));

                    double error_percent = 100 - (match_percent + match_percent1);
                    Statistics.Items.Add(String.Format("\n Mismatch percent: {0:0.#}%", error_percent));
                    if (error_percent > 30)
                        ProgressBar.Background = new SolidColorBrush(Colors.Red);
                    else
                        ProgressBar.Background = new SolidColorBrush(Colors.Green);
                }
                else
                    MessageBox.Show("Please, enter Id or pick a friend");
            }
            else
                MessageBox.Show("You didn't sign in");

        }


        private event Action Ev;
        private void FriendsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!VkOn.IsEnabled)
            {
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    var u = (User)FriendsComboBox.SelectedItem;
                    VkRepository.Instance.RequestedUserID = u.Uid;
                    UserIdTextBlock.Text = u.Uid;
                }));
            }
        }

        private void VkOn_Click(object sender, RoutedEventArgs e)
        {
            VkOn.IsEnabled = false;
            FBOn.IsEnabled = true;
            Login.IsEnabled = true;
        }

        private void FBOn_Click(object sender, RoutedEventArgs e)
        {
            VkOn.IsEnabled = true;
            FBOn.IsEnabled = false;
            Login.IsEnabled = false;
            AuthInfo.Text = "";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Auth();
            Login.IsEnabled = false;
        }

        private void OpenIdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (r1.SignedIn == true)
            {
                UserIdTextBlock.Visibility = Visibility.Visible;
                FriendsComboBox.Visibility = Visibility.Hidden;
            }
            else
                MessageBox.Show("You didn't sign in");
        }

        private void OpenFriendsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (r1.SignedIn == true)
            {
                if (!VkOn.IsEnabled)
                {
                    if (FriendsComboBox.HasItems == false)
                        FriendsComboBox.ItemsSource = VkRepository.Instance.GetFriends();
                }
                UserIdTextBlock.Visibility = Visibility.Hidden;
                FriendsComboBox.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("You didn't sign in");
        }

        private void DownloadProfilePhoto_Click(object sender, RoutedEventArgs e)
        {
            if (r1.SignedIn == true)
            {
                if (!VkOn.IsEnabled)
                {
                    User u = VkRepository.GetUserInfo(VkRepository.Instance.RequestedUserID, "photo_200");
                    VkRepository.DownloadFile(u.Photo);
                }
            }
            else
                MessageBox.Show("You didn't sign in");
        }


    }
}
