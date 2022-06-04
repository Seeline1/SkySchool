using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public static SkySchoolEntities _coontext = new SkySchoolEntities();

        public PageUser()
        {
            InitializeComponent();
            TBLogin.Text = Manager.CurrentUser.Login.ToString();
            TBName.Text = Manager.CurrentUser.FIO.ToString();
            TBParol.Text = "**********";
            TBRole.Text = Manager.CurrentUser.Role.ToString();
            DGridDisciplini.ItemsSource = _coontext.SpisokDisciplin.Where(h => h.User.Login == TBLogin.Text).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplini.ItemsSource = _coontext.SpisokDisciplin.Where(h => h.User.Login == TBLogin.Text).ToList();
            }
        }

        private void ImgOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти из профиля?", "Внимание",
                 MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Manager.MainFrame.Navigate(new PageAuth());
            }
        }

        private void CheckPass_Checked(object sender, RoutedEventArgs e)
        {
            if (TBParol.Text == Manager.CurrentUser.Parol.ToString())
            {
                TBParol.Text = "**********";
            }
        }

        private void CheckPass_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TBParol.Text == "**********")
            {
                TBParol.Text = Manager.CurrentUser.Parol.ToString();
            }
        }

        private void MyPlans_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
