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
    public partial class AddEditPageDisciplina : Page
    {
        public Disciplina _currentDis = new Disciplina();

        public AddEditPageDisciplina(Disciplina selectedDis)
        {
            InitializeComponent();

            if (selectedDis != null)
                _currentDis = selectedDis;

            DataContext = _currentDis;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentDis.Nazvanie))
                errors.AppendLine("Укажите название");

            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                Disciplina tempDis = db.Disciplina.Where(p => p.Nazvanie.Equals(_currentDis.Nazvanie)).FirstOrDefault();
                if (TxtDis.Text == _currentDis.Nazvanie) { }
                else if (tempDis != null)
                {
                    errors.AppendLine("Такая дисциплина уже существует");
                }
            }

            if (TxtDis.Text.Length <= 100)
            {
                bool ru = true;

                for (int i = 0; i < TxtDis.Text.Length; i++)
                {
                    if (TxtDis.Text[i] >= 'A' && TxtDis.Text[i] <= 'Z')
                        ru = false;

                    if (TxtDis.Text[i] >= 'a' && TxtDis.Text[i] <= 'z')
                        ru = false;
                }

                if (!ru)
                    errors.AppendLine("Доступна только русская раскладка");
            }
            else
            {
                errors.AppendLine("Название должно содержать до 100 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentDis.ID_Disciplina == 0)
                Manager.GetContext().Disciplina.Add(new Disciplina() { Nazvanie = TxtDis.Text });

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
