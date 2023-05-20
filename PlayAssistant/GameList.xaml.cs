using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameList.xaml
/// </summary>
public partial class GameList : Page
{
    private readonly List<GameBtn> _btnList = new();

    public GameList(List<string> titles)
    {
        InitializeComponent();

        foreach (var title in titles)
            _btnList.Add(new GameBtn(title));

        for (var i = 0; i < titles.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            _btnList[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(_btnList[i]);
        }
    }

    public void Search(string name)
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < _btnList.Count; i++)
            if (_btnList[i].Game.ToLower().Contains(name.Trim().ToLower()))
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                _btnList[i].SetValue(Grid.ColumnProperty, i);
                MainGrid.Children.Add(_btnList[i]);
            }
    }

    public void ResetSearch()
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < _btnList.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            _btnList[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(_btnList[i]);
        }
    }
}