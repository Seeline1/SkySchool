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
    public partial class PageInfoStudent : Page, IDisposable
    {
        public readonly SkySchoolEntities _context;

        public PageInfoStudent()
        {
            InitializeComponent();

            _context = new SkySchoolEntities();

            DGridStudent.ItemsSource = _context.Student.OrderBy(h => h.ID_Gruppi).ToList();
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridStudent.ItemsSource = _context.Student.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageStudent(new Student()));
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var StudentsForRemoving = DGridStudent.SelectedItems.Cast<Student>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {StudentsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Student.RemoveRange(StudentsForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridStudent.ItemsSource = _context.Student.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageStudent((sender as Button).DataContext as Student));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
