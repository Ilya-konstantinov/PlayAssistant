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
    /// Логика взаимодействия для GameBtn.xaml
    /// </summary>
    public partial class GameBtn : UserControl
    {
        public string game = "TestGame";

        public GameBtn(string _game_name = "Game")
        {
            InitializeComponent();

            game = _game_name.ToLower();

            Select_btn.Content = _game_name;
        }

        private void Select_btn_Click(object sender, RoutedEventArgs e)
        {
            SessionService.SessionName= game;
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
            parentWindow.StartSession();
        }
    }
}
