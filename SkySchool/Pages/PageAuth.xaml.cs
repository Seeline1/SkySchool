using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAuth.xaml
    /// </summary>
    public partial class PageAuth : Page
    {
        public PageAuth()
        {
            InitializeComponent();
        }

        private void PassBoxParol_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PassBoxParol.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
            }
            else
            {
                ImgShowHide.Visibility = Visibility.Hidden;
            }
        }

        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        void ShowPassword()
        {
            TxtParol.Visibility = Visibility.Visible;
            PassBoxParol.Visibility = Visibility.Hidden;
            TxtParol.Text = PassBoxParol.Password;
        }
        void HidePassword()
        {
            TxtParol.Visibility = Visibility.Hidden;
            PassBoxParol.Visibility = Visibility.Visible;
            PassBoxParol.Focus();
        }

        private async void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            TxtParol.Text = PassBoxParol.Password;
            PassBoxParol.Password = TxtParol.Text;
            if (await EntryCheck.Login(TxtLogin.Text, TxtParol.Text))
            {
                if (Manager.CurrentUser.Role == "admin")
                {
                    Manager.MainFrame.Navigate(new PageAdmin());
                }
                else
                {
                    Manager.MainFrame.Navigate(new PageUser());
                }
            }
        }
    }
}
