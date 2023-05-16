using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlayAssistant
{
    /// <summary>
    /// Логика взаимодействия для GameCreateMenu.xaml
    /// </summary>
    public partial class GameCreateMenu : UserControl
    {
        public GameCreateMenu()
        {
            InitializeComponent();
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
        }

        private void Create_btn_Click(object sender, RoutedEventArgs e)
        {
            ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
            SessionService.CreateSession(ElTitle.Text);
            ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
        }
    }
}
