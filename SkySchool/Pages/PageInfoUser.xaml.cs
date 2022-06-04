using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageInfo.xaml
    /// </summary>
    public partial class PageInfoUser : Page
    {
        public static SkySchoolEntities _coontext = new SkySchoolEntities();

        public PageInfoUser()
        {
            InitializeComponent();
            DGridUser.ItemsSource = _coontext.User.OrderBy(h => h.FIO).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridUser.ItemsSource = Manager.GetContext().User.ToList();
            }
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageUser(new User()));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var UsersForRemoving = DGridUser.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {UsersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Manager.GetContext().User.RemoveRange(UsersForRemoving);
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridUser.ItemsSource = Manager.GetContext().User.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageUser((sender as Button).DataContext as User));
        }
    }
}
