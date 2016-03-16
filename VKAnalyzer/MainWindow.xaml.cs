using System;
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
        private string _r = "Vk";
        public MainWindow()
        {
            InitializeComponent();
            
            Ev += () =>
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    WaitProgressBar.IsIndeterminate = true;
                }));
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
            VkRepository.Instance.RequestedUserID = UserIdTextBlock.Text;
            ListBox.ItemsSource = VkRepository.Instance.Compare_groups();
        }

        private void FriendsComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (FriendsComboBox.HasItems == false)
                    FriendsComboBox.ItemsSource = VkRepository.Instance.GetFriends();
        }

        private event Action Ev;
        private void FriendsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var t = new Thread(() => WaitProgressBar.IsIndeterminate = true);
            
            Dispatcher.BeginInvoke(new Action(delegate()
            {
                
                    //
                    VkRepository.total = 0;
                    VkRepository.pluses = 0;
                    VkRepository.small = 0;
                    VkRepository.exclam = 0;
                    //
                    var u = (User)FriendsComboBox.SelectedItem;
                    VkRepository.Instance.RequestedUserID = u.Uid;
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

                }));
        }

        private void VkOn_Click(object sender, RoutedEventArgs e)
        {
            _r = "Vk";
            VkOn.Background = Brushes.Green;
            FBOn.Background = Brushes.White;
        }

        private void FBOn_Click(object sender, RoutedEventArgs e)
        {
            _r = "FB";
            FBOn.Background = Brushes.Green;
            VkOn.Background = Brushes.White;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Auth();
            Login.IsEnabled = false;
        }


    }
}
