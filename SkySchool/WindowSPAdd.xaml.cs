using System;
using System.Data.Entity;
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
    public partial class WindowSPAdd : Window, IDisposable
    {
        private SpisokDisciplin _currentSP;
        public readonly SkySchoolEntities _context = new SkySchoolEntities();
        private int idu = 0;

        public WindowSPAdd(int id)
        {
            InitializeComponent();
            idu = id;
            _currentSP = new SpisokDisciplin();
            _context = new SkySchoolEntities();

            DataContext = _currentSP;
            ComboBoxD.ItemsSource = _context.Disciplina.ToList();
            ComboBoxT.ItemsSource = _context.Tip_Zanyatiya.ToList();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            Disciplina dis = ComboBoxD.SelectedItem as Disciplina;
            Tip_Zanyatiya tip = ComboBoxT.SelectedItem as Tip_Zanyatiya;

            if (dis != null)
            {
                if(tip != null)
                {
                    SpisokDisciplin tempSD = await _context.SpisokDisciplin.FirstOrDefaultAsync(p => p.ID_User.Equals(idu) && p.ID_Disciplina.Equals(dis.ID_Disciplina) && p.ID_Tip_Zanyatiya.Equals(tip.ID_Tip_Zanyatiya));

                    if (tempSD != null)
                    {
                        errors.AppendLine("Эта дисциплина уже закреплена за преподавателем");
                    }
                }
                else
                {
                    errors.AppendLine("Выберите вид занятия");
                }
            }
            else
            {
                errors.AppendLine("Выберите дисциплину");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _context.SpisokDisciplin.Add(new SpisokDisciplin()
            {
                ID_User = idu, Disciplina = ComboBoxD.SelectedItem as Disciplina, Tip_Zanyatiya = ComboBoxT.SelectedItem as Tip_Zanyatiya
            });

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
