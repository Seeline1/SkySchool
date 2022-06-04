using SkySchool.Classes;
using SkySchool.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkySchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageInfoGruppa.xaml
    /// </summary>
    public partial class PageInfoGruppa : Page
    {
        public static SkySchoolEntities _coontext = new SkySchoolEntities();

        public PageInfoGruppa()
        {
            InitializeComponent();
            DGridGr.ItemsSource = _coontext.Gruppa.OrderBy(h => h.Nomer_Gruppi).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridGr.ItemsSource = Manager.GetContext().Gruppa.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageGruppa(new Gruppa()));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var GrForRemoving = DGridGr.SelectedItems.Cast<Gruppa>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {GrForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Manager.GetContext().Gruppa.RemoveRange(GrForRemoving);
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridGr.ItemsSource = Manager.GetContext().Gruppa.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
