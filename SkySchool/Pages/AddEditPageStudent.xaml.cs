using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class AddEditPageStudent : Page, IDisposable
    {
        private readonly SkySchoolEntities _context;
        private Student _currentStud;
        private string fio = "";
        private int ng = 0;

        public AddEditPageStudent(Student selectedStud)
        {
            InitializeComponent();

            _currentStud = new Student();

            if (selectedStud != null)
                _currentStud = selectedStud;

            _context = new SkySchoolEntities();

            DataContext = _currentStud;
            TxtFIO.Text = _currentStud.FIO;
            ComboBoxNG.ItemsSource = _context.Gruppa.ToList();
            fio = _currentStud.FIO;
            ng = _currentStud.ID_Gruppi;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                StringBuilder errors = new StringBuilder();

                Student tempStud = await _context.Student.FirstOrDefaultAsync(p => p.FIO.Equals(_currentStud.FIO));

                if (TxtFIO.Text == fio) { }
                else if (tempStud != null)
                {
                    Student tempNG = await _context.Student.FirstOrDefaultAsync(p => p.ID_Gruppi.Equals(_currentStud.ID_Gruppi));
                    
                    if (ComboBoxNG.Text == Convert.ToString(ng)) { }
                    else if (tempNG != null)
                    {
                        errors.AppendLine("Такой студент уже существует");
                    }
                }

                if (string.IsNullOrWhiteSpace(_currentStud.FIO))
                {
                    errors.AppendLine("Укажите ФИО");
                }    

                if (string.IsNullOrWhiteSpace(Convert.ToString(_currentStud.ID_Gruppi)))
                {
                    errors.AppendLine("Укажите номер группы");
                }

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

                Gruppa gupp = ComboBoxNG.SelectedItem as Gruppa;

                if (_currentStud.ID_Student == 0)
                {
                    _context.Student.Add(new Student()
                    { FIO = TxtFIO.Text, ID_Gruppi = gupp.ID_Gruppi });
                }
                else
                {
                    var studentToUpdate = await _context.Student.FirstOrDefaultAsync(x => x.ID_Student == _currentStud.ID_Student);
                    studentToUpdate.ID_Gruppi = gupp.ID_Gruppi;
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