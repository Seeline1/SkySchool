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
    /// Логика взаимодействия для PageInfoGruppa.xaml
    /// </summary>
    public partial class PageInfoGruppa : Page, IDisposable
    {
        private readonly SkySchoolEntities _context;

        public PageInfoGruppa()
        {
            InitializeComponent();

            _context = new SkySchoolEntities();

            DGridGr.ItemsSource = _context.Gruppa.OrderBy(h => h.Nomer_Gruppi).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridGr.ItemsSource = _context.Gruppa.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageGruppa());
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var GrForRemoving = DGridGr.SelectedItems.Cast<Gruppa>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {GrForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Gruppa.RemoveRange(GrForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridGr.ItemsSource = _context.Gruppa.ToList();
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
