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
    public partial class PageInfoDisciplina : Page
    {
        public static SkySchoolEntities _coontext = new SkySchoolEntities();

        public PageInfoDisciplina()
        {
            InitializeComponent();
            DGridDis.ItemsSource = _coontext.Disciplina.OrderBy(h => h.Nazvanie).ToList();
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDis.ItemsSource = Manager.GetContext().Disciplina.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageDisciplina(new Disciplina()));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var DisForRemoving = DGridDis.SelectedItems.Cast<Disciplina>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {DisForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Manager.GetContext().Disciplina.RemoveRange(DisForRemoving);
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridDis.ItemsSource = Manager.GetContext().Disciplina.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageDisciplina((sender as Button).DataContext as Disciplina));
        }
    }
}
