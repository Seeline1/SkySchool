using System.Windows;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Navigate(new PageAuth());
        }
    }
}