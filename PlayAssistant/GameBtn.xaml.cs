using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameBtn.xaml
/// </summary>
public partial class GameBtn : UserControl
{
    public string game = "TestGame";

    public GameBtn(string _game_name = "Game")
    {
        InitializeComponent();

        game = _game_name;

        Select_btn.Content = _game_name;
    }

    private void Select_btn_Click(object sender, RoutedEventArgs e)
    {
        SessionService.SessionName = game;
        var parentWindow = Window.GetWindow(this) as MainWindow;
        ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
        parentWindow.StartSession();
    }
}