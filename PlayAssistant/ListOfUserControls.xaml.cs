using System;
using System.Collections.Generic;
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
    public Character curCh;
    public bool InMainWindow;
    private readonly bool IsPSList;

    public ListOfUserControls(List<IReturnValue> userControls, bool _IsPSList, bool _InMainWindow,
        Character _curCh = null)
    {
        InitializeComponent();
        foreach (var item in userControls) MainList.Items.Add(item);

        IsPSList = _IsPSList;
        InMainWindow = _InMainWindow;
        curCh = _curCh;
    }

    private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        var selected = (IReturnValue)MainList.SelectedItem;
        if (selected == null)
            return;
        selected.Title = ElementTitle.Text;
        if (IsPSList)
        {
            var instance = Activator.CreateInstance(
                selected.GetType(),
                selected.Title,
                "");
            ((UserControl)instance).SetValue(Grid.ColumnProperty, 1);
            ((UserControl)instance).SetValue(Grid.RowProperty, 1);
            parentWindow.AddPS((IReturnValue)
                instance
            );
        }
        else
        {
            if (curCh == null)
                Character.AddGeneralAttributes(selected);
            else
                curCh.AddAttribute(selected);

            parentWindow.Refrash();
        }

        parentWindow.RemoveList(InMainWindow);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.RemoveList(InMainWindow);
    }
}