using System;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для AddEditPageGruppa.xaml
    /// </summary>
    public partial class AddEditPageGruppa : Page, IDisposable
    {
        private readonly SkySchoolEntities _context;
        private Gruppa _currentGr;

        public AddEditPageGruppa(Gruppa selectedGr)
        {
            InitializeComponent();

            _currentGr = new Gruppa();

            if (selectedGr != null)
            {
                _currentGr = selectedGr;
            }

            _context = new SkySchoolEntities();

            TxtNG.Text = Convert.ToString(_currentGr.Nomer_Gruppi);
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

                if (TxtNG.Text.Length == 4)
                {
                    for (int i = 0; i < TxtNG.Text.Length; i++)
                    {
                        if (TxtNG.Text[i] >= '0' && TxtNG.Text[i] <= '9') { }
                        else
                        {
                            errors.AppendLine("Номер группы должен состоять только из цифр");
                            break;
                        }
                    }
                }
                else
                {
                    errors.AppendLine("Номер группы должен состоять из 4 цифр");
                }

                int a = Convert.ToInt32(TxtNG.Text);

                Gruppa tempGr = await db.Gruppa.FirstOrDefaultAsync(p => p.Nomer_Gruppi.Equals(a));

                if (tempGr != null)
                {
                    MessageBox.Show("Группа с таким номером уже существует");
                    return;
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                _context.Gruppa.Add(new Gruppa()
                {
                    Nomer_Gruppi = Convert.ToInt32(TxtNG.Text)
                });

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
