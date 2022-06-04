using System;
using System.Linq;
using System.Text;
using System.Windows;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool
{
    /// <summary>
    /// Логика взаимодействия для WindowSPAdd.xaml
    /// </summary>
    public partial class WindowSPAdd : Window
    {
        private SpisokDisciplin _currentSP = new SpisokDisciplin();

        public WindowSPAdd(SpisokDisciplin selectedSP)
        {
            InitializeComponent();

            if (selectedSP != null)
                _currentSP = selectedSP;

            DataContext = _currentSP;
            ComboBoxD.ItemsSource = Manager.GetContext().Disciplina.ToList();
            ComboBoxT.ItemsSource = Manager.GetContext().Tip_Zanyatiya.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentSP.ID_Disciplina)))
                errors.AppendLine("Укажите дисциплину");
            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentSP.ID_Tip_Zanyatiya)))
                errors.AppendLine("Укажите вид занятия");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentSP.ID_SpisokDisciplin == 0)
                Manager.GetContext().SpisokDisciplin.Add(new SpisokDisciplin() { ID_User = _currentSP.ID_User, Disciplina = ComboBoxD.SelectedItem as Disciplina, Tip_Zanyatiya = ComboBoxT.SelectedItem as Tip_Zanyatiya });

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
    }
}
