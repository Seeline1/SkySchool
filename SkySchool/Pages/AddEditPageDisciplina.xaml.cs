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

        public AddEditPageDisciplina(Disciplina selectedDis)
        {
            InitializeComponent();
            
            _currentDis = new Disciplina();

            if (selectedDis != null)
            {
                _currentDis = selectedDis;
            }

            _context = new SkySchoolEntities();

            string d = _currentDis.Nazvanie;
            TxtDis.Text = d;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("2" + _currentDis.Nazvanie);
            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                StringBuilder errors = new StringBuilder();

                if (string.IsNullOrWhiteSpace(TxtDis.Text))
                {
                    errors.AppendLine("Укажите название");
                }
                //MessageBox.Show("1" + _currentDis.Nazvanie);

                string a = TxtDis.Text;

                Disciplina tempDis = await db.Disciplina.FirstOrDefaultAsync(p => p.Nazvanie.Equals(a));
                
                if (tempDis != null)
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
                    db.Disciplina.Add(new Disciplina()
                    { Nazvanie = TxtDis.Text });
                }

                try
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Информация сохранена!");
                    Manager.MainFrame.GoBack();
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
    }
}