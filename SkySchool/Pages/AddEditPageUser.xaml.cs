using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPageUser : Page, IDisposable
    {
        private readonly SkySchoolEntities _context;
        private User _currentUser;
        private string log = "";

        public AddEditPageUser(User selectedUser)
        {
            InitializeComponent();

            _currentUser = new User();
            _context = new SkySchoolEntities();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
            }

            DataContext = _currentUser;
            TxtFIO.Text = _currentUser.FIO;
            TxtRole.Text = _currentUser.Role;
            TxtLog.Text = _currentUser.Login;
            TxtPass.Text = _currentUser.Parol;
            DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
            log = _currentUser.Login;
            BtnVisivle();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            User tempUser = await _context.User.FirstOrDefaultAsync(p => p.Login.Equals(_currentUser.Login));
                    
            if (TxtLog.Text == log) { }
            else if (tempUser != null)
            {
                errors.AppendLine("Пользователь с таким логином уже существует");
            }
                
            if (TxtFIO.Text.Length < 150)
            {
                for (int i = 0; i < TxtFIO.Text.Length; i++)
                {
                    if (TxtFIO.Text[i] >= '0' && TxtFIO.Text[i] <= '9')
                    {
                        errors.AppendLine("ФИО не должно содержать цифр");
                    }
                }

                if (string.IsNullOrWhiteSpace(TxtFIO.Text))
                {
                    errors.AppendLine("Укажите ФИО");
                }
            }
            else
            {
                errors.AppendLine("ФИО должно содержать не более 150 символов");
            }

            if (TxtRole.Text == "user" || TxtRole.Text == "admin") { }
            else
            {
                errors.AppendLine("Укажите роль: \"admin\" или \"user\"");
            }

            if (TxtLog.Text.Length == 11)
            {
                for (int i = 0; i < TxtLog.Text.Length; i++)
                {
                    if (TxtLog.Text[i] >= '0' && TxtLog.Text[i] <= '9') { }
                    else
                    {
                        errors.AppendLine("Логин должен состоять только из цифр");
                        break;
                    }
                }
            }
            else
            {
                errors.AppendLine("Логин должен состоять из 11 цифр");
            }

            if (TxtPass.Text.Length >= 6 && TxtPass.Text.Length <= 15)
            {
                bool en = true;
                bool num = false;
                bool symbol = false;

                for (int i = 0; i < TxtPass.Text.Length; i++)
                {
                    if (TxtPass.Text[i] >= 'А' && TxtPass.Text[i] <= 'Я')
                    {
                        en = false;
                    }

                    if (TxtPass.Text[i] >= 'а' && TxtPass.Text[i] <= 'я')
                    {
                        en = false;
                    }

                    if (TxtPass.Text[i] >= '0' && TxtPass.Text[i] <= '9')
                    {
                        num = true;
                    }

                    if (TxtPass.Text[i] == '!' || TxtPass.Text[i] == '@' || TxtPass.Text[i] == '$')
                    {
                        symbol = true;
                    }
                }

                if (!en)
                {
                    errors.AppendLine("Доступна только английская раскладка");
                }

                if (!num)
                {
                    errors.AppendLine("Добавьте хотя бы одну цифру");
                }

                if (!symbol)
                {
                    errors.AppendLine("Добавьте один из следующих символов: \"!\" или \"@\" или \"$\"");
                }
            }
            else
            {
                errors.AppendLine("Пароль должен содержать от 6 до 15 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID_User == 0)
            {
                _context.User.Add(new User()
                { FIO = TxtFIO.Text, Login = TxtLog.Text, Parol = TxtPass.Text, Role = TxtRole.Text });
            }
            else
            {
                var userToUpdate = await _context.User.FirstOrDefaultAsync(x => x.ID_User == _currentUser.ID_User);
                userToUpdate.Login = TxtLog.Text;
                userToUpdate.Parol = TxtPass.Text;
                userToUpdate.Role = TxtRole.Text;
                userToUpdate.FIO = TxtFIO.Text;
            }

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        private void ImgAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowSPAdd Windowspadd = new WindowSPAdd(_currentUser.ID_User);
            Windowspadd.Show();
        }

        private async void ImgDel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var UsersDis = DGridDisciplini.SelectedItems.Cast<SpisokDisciplin>().ToList();

            if (UsersDis.Count == 0)
            {
                MessageBox.Show("Выберите элементы для удаления");
            }
            else if (MessageBox.Show($"Вы точно хотите удалить следующи {UsersDis.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.SpisokDisciplin.RemoveRange(UsersDis);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void ImgUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
            }
        }

        private void BtnVisivle()
        {
            if(_currentUser.ID_User == 0)
            {
                ImgAdd.Visibility = Visibility.Hidden;
                ImgDel.Visibility = Visibility.Hidden;
                ImgUpdate.Visibility = Visibility.Hidden;
            }
        }
    }
}