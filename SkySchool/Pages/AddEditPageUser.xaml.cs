﻿using System;
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

        public AddEditPageUser(User selectedUser)
        {
            InitializeComponent();

            _currentUser = new User();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
            }

            TxtFIO.Text = _currentUser.FIO;
            TxtRole.Text = _currentUser.Role;
            TxtLog.Text = _currentUser.Login;
            TxtPass.Text = _currentUser.Parol;
            DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                if (Visibility == Visibility.Visible)
                {
                    db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    DGridDisciplini.ItemsSource = _context.SpisokDisciplin.Where(h => h.User.Login == _currentUser.Login).ToList();
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                StringBuilder errors = new StringBuilder();

                User tempUser = await db.User.FirstOrDefaultAsync(p => p.Login.Equals(_currentUser.Login));
                    
                if (TxtLog.Text == _currentUser.Login) { }
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
                    db.User.Add(new User()
                    { FIO = TxtFIO.Text, Login = TxtLog.Text, Parol = TxtPass.Text, Role = TxtRole.Text });
                }

                try
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ImgAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowSPAdd Windowspadd = new WindowSPAdd(new SpisokDisciplin()/* { ID_User = _currentUser.ID_User, Disciplina = new Disciplina(), Tip_Zanyatiya = new Tip_Zanyatiya() }*/);
            Windowspadd.Show();
        }

        private void ImgDel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var UsersDis = DGridDisciplini.SelectedItems.Cast<SpisokDisciplin>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {UsersDis.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SkySchoolEntities db = new SkySchoolEntities())
                    {
                        db.SpisokDisciplin.RemoveRange(UsersDis);
                        db.SaveChanges();
                        MessageBox.Show("Данные удалены!");

                        DGridDisciplini.ItemsSource = db.SpisokDisciplin.ToList();

                    }
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
    }
}