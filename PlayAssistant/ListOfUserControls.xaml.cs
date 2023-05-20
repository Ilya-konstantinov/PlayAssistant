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
public partial class ListOfUserControls : UserControl
{
    public readonly Character CurCh;
    public readonly bool InMainWindow;
    private readonly bool _isPsList;

    public ListOfUserControls(List<IReturnValue> userControls, bool isPsList, bool inMainWindow,
        Character curCh = null)
    {
        InitializeComponent();
        foreach (var item in userControls) MainList.Items.Add(item);

        _isPsList = isPsList;
        InMainWindow = inMainWindow;
        CurCh = curCh;

        var t = MainList.Items.OfType<UIElement>().ToList();
        foreach (var item in t)
            item.IsEnabled = false;

    }

    private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        var selected = (IReturnValue)MainList.SelectedItem;
        if (selected == null)
            return;
        selected.Title = ElementTitle.Text;
        if (_isPsList)
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
            if (CurCh == null)
                Character.AddGeneralAttributes(selected);
            else
                CurCh.AddAttribute(selected);

            parentWindow.Refrash();
        }

        parentWindow.RemoveList(InMainWindow);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.RemoveList(InMainWindow);
        parentWindow.CloseOverlayed();
    }
}