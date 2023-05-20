using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameChooseMenu.xaml
/// </summary>
public partial class GameChooseMenu : Page
{
    public GameChooseMenu(List<string> titles)
    {
        InitializeComponent();

        var gameList = new GameList(titles);
        GameList_frame.Content = gameList;
    }

    public void SetPage(TestGamePage _page)
    {
        GameList_frame.Content = _page;
    }

    public void Stels()
    {
        Hide.IsEnabled = true;
        Hide.Visibility = Visibility.Visible;
    }

    public void UnStels()
    {
        Hide.IsEnabled = false;
        Hide.Visibility = Visibility.Hidden;
    }

    public void OpenGameCreate(object sender, RoutedEventArgs e)
    {
        Stels();
        GameCreate_grid.Visibility = Visibility.Visible;
        GameCreate_grid.IsEnabled = true;
        var gcm = new GameCreateMenu();

        Grid.SetRow(gcm, 1);
        Grid.SetColumn(gcm, 1);

        GameCreate_grid.Children.Add(gcm);
    }

    public void CloseGameCreate()
    {
        GameCreate_grid.Visibility = Visibility.Hidden;
        GameCreate_grid.IsEnabled = false;

        GameCreate_grid.Children.Clear();

        UnStels();
    }

    private void Search_btn_Click(object sender, RoutedEventArgs e)
    {
        if (Search_textbox.Text.Trim() == "")
            ((GameList)GameList_frame.Content).ResetSearch();
        else
            ((GameList)GameList_frame.Content).Search(Search_textbox.Text.Trim());
    }

    private void Search_textbox_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;

        if (Search_textbox.Text.Trim() == "")
            ((GameList)GameList_frame.Content).ResetSearch();
        else
            ((GameList)GameList_frame.Content).Search(Search_textbox.Text.Trim());
    }
}