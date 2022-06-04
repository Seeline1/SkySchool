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
    /// Логика взаимодействия для AddEditPageGruppa.xaml
    /// </summary>
    public partial class AddEditPageGruppa : Page
    {
        public Gruppa _currentGr = new Gruppa();

        public AddEditPageGruppa(Gruppa selectedGr)
        {
            InitializeComponent();
            if (selectedGr != null)
                _currentGr = selectedGr;

            DataContext = _currentGr;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (TxtGr.Text.Length == 4)
            {
                for (int i = 0; i < TxtGr.Text.Length; i++)
                {
                    if (TxtGr.Text[i] >= '0' && TxtGr.Text[i] <= '9') { }
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

            using (SkySchoolEntities db = new SkySchoolEntities())
            {
                int a = Convert.ToInt32(TxtGr.Text);
                Gruppa tempGr = db.Gruppa.Where(p => p.Nomer_Gruppi.Equals(a)).FirstOrDefault();
                if (tempGr != null)
                {
                    MessageBox.Show("Группа с таким номером уже существует");
                    return;
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentGr.ID_Gruppi == 0)
                Manager.GetContext().Gruppa.Add(new Gruppa() { Nomer_Gruppi = Convert.ToInt32(TxtGr.Text) });

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
