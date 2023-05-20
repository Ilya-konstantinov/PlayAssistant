using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameList.xaml
/// </summary>
public partial class GameList : Page
{
    private readonly List<GameBtn> btn_list = new();

    public GameList(List<string> titles)
    {
        InitializeComponent();

        foreach (var title in titles)
            btn_list.Add(new GameBtn(title));

        for (var i = 0; i < titles.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            btn_list[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(btn_list[i]);
        }
    }

    public void Search(string name)
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < btn_list.Count; i++)
            if (btn_list[i].game.ToLower().Contains(name.Trim().ToLower()))
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                btn_list[i].SetValue(Grid.ColumnProperty, i);
                MainGrid.Children.Add(btn_list[i]);
            }
    }

    public void ResetSearch()
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < btn_list.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            btn_list[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(btn_list[i]);
        }
    }
}