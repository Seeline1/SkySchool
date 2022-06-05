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
    /// Логика взаимодействия для AddEditPageDis1.xaml
    /// </summary>
    public partial class AddEditPageDisciplina : Page, IDisposable
    {
        private readonly SkySchoolEntities _context;
        private Disciplina _currentDis;
        private string dis = "";

        public AddEditPageDisciplina(Disciplina selectedDis)
        {
            InitializeComponent();
            
            _currentDis = new Disciplina();
            _context = new SkySchoolEntities();

            if (selectedDis != null)
            {
                _currentDis = selectedDis;
            }

            DataContext = _currentDis;
            TxtDis.Text = _currentDis.Nazvanie;
            dis = _currentDis.Nazvanie;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TxtDis.Text))
            {
                errors.AppendLine("Укажите название");
            }

            Disciplina tempDis = await _context.Disciplina.FirstOrDefaultAsync(p => p.Nazvanie.Equals(_currentDis.Nazvanie));
            if (TxtDis.Text == dis) { }
            else if (tempDis != null)
            {
                errors.AppendLine("Такая дисциплина уже существует");
            }

            if (TxtDis.Text.Length <= 100)
            {
                bool ru = true;

                for (int i = 0; i < TxtDis.Text.Length; i++)
                {
                    if (TxtDis.Text[i] >= 'A' && TxtDis.Text[i] <= 'Z')
                    {
                        ru = false;
                    }

                    if (TxtDis.Text[i] >= 'a' && TxtDis.Text[i] <= 'z')
                    {
                        ru = false;
                    }
                }

                if (!ru)
                {
                    errors.AppendLine("Доступна только русская раскладка");
                }
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
            {
                _context.Disciplina.Add(new Disciplina()
                { Nazvanie = TxtDis.Text });
            }
            else
            {
                var disciplinaToUpdate = await _context.Disciplina.FirstOrDefaultAsync(x => x.ID_Disciplina == _currentDis.ID_Disciplina);
                disciplinaToUpdate.ID_Disciplina = _currentDis.ID_Disciplina;
            }

            try
            {
                await _context.SaveChangesAsync();
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