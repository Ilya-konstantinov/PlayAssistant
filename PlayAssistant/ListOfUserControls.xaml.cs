using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ServiceLibrary;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для ListOfUserControls.xaml
/// </summary>
public partial class ListOfUserControls
{
    private readonly bool _isPlayStaticList;
    private readonly Character _currentCharacter;
    private readonly bool _inMainWindow;

    public ListOfUserControls(List<IReturnValue> userControls, bool isPlayStaticList, bool inMainWindow,
        Character currentCharacter = null)
    {
        InitializeComponent();
        foreach (var item in userControls) MainList.Items.Add(item);

        _isPlayStaticList = isPlayStaticList;
        _inMainWindow = inMainWindow;
        _currentCharacter = currentCharacter;

        var modules = MainList.Items.OfType<UIElement>().ToList();
        foreach (var item in modules)
            item.IsEnabled = false;
    }

    private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        var selected = (IReturnValue)MainList.SelectedItem;
        if (selected == null)
            return;
        selected.Title = ElementTitle.Text;
        if (_isPlayStaticList)
        {
            var instance = Activator.CreateInstance(
                selected.GetType(),
                selected.Title,
                "");
            ((UserControl)instance).SetValue(Grid.ColumnProperty, 1);
            ((UserControl)instance).SetValue(Grid.RowProperty, 1);
            parentWindow.AddPs((IReturnValue)
                instance
            );
        }
        else
        {
            if (_currentCharacter == null)
                Character.AddGeneralAttributes(selected);
            else
                _currentCharacter.AddAttribute(selected);

            parentWindow.Refrash();
        }

        parentWindow.RemoveList(_inMainWindow);
    }

    private void CloseButtonClick(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.RemoveList(_inMainWindow);
        parentWindow.CloseOverlayed();
    }
}