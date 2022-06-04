using System;
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
    /// Логика взаимодействия для AddEditPageDis1.xaml
    /// </summary>
    public partial class AddEditPageStudent : Page
    {
        public Student _currentStud = new Student();
        public string st = "";
        public int gr = 0;

        public AddEditPageStudent(Student selectedStud)
        {
            InitializeComponent();

            if (selectedStud != null)
                _currentStud = selectedStud;

            DataContext = _currentStud;
            ComboBoxNG.ItemsSource = Manager.GetContext().Gruppa.ToList();
            st = _currentStud.FIO;
            gr = _currentStud.ID_Gruppi;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                Student tempStud = db.Student.Where(p => p.FIO.Equals(_currentStud.FIO)).FirstOrDefault();
                if (TxtFIO.Text == st) { }
                else if (tempStud != null)
                {
                    Student tempStud1 = db.Student.Where(p => p.ID_Gruppi.Equals(_currentStud.ID_Gruppi)).FirstOrDefault();
                    if (ComboBoxNG.Text == Convert.ToString(gr)) { }
                    else if (tempStud1 != null)
                    {
                        errors.AppendLine("Такой студент уже существует");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(_currentStud.FIO))
                errors.AppendLine("Укажите ФИО");

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentStud.ID_Gruppi)))
                errors.AppendLine("Укажите номер группы");

            if (TxtFIO.Text.Length < 150)
            {
                for (int i = 0; i < TxtFIO.Text.Length; i++)
                {
                    if (TxtFIO.Text[i] >= '0' && TxtFIO.Text[i] <= '9')
                    {
                        errors.AppendLine("ФИО может содержать только буквы");
                        break;
                    }
                }
            }
            else
            {
                errors.AppendLine("Фио не может содержать более 150 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentStud.ID_Student == 0)
            {
                Gruppa kk = ComboBoxNG.SelectedItem as Gruppa;
                Manager.GetContext().Student.Add(new Student() { FIO = TxtFIO.Text, ID_Gruppi = kk.ID_Gruppi/*ComboBoxNG.SelectedItem as Gruppa*/ });
            }

                try
                {
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}