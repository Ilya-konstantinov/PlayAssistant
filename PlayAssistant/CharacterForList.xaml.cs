using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ServiceLibrary;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для CharacterForList.xaml
/// </summary>
public enum Status
{
    Open,
    Close
}

public partial class CharacterForList
{
    private Status _status = Status.Close;
    public readonly Character Character;

    internal CharacterForList(Character character)
    {
        InitializeComponent();
        Character = character;
        Name.Content = character.Name;
        Avatar.Source = new BitmapImage(new Uri(character.AvatarPath, UriKind.Absolute));
    }

    private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_status == Status.Open)
            Close();
        else
            Open();
    }

    private void Close()
    {
        _status = Status.Close;
        Height /= 2;
        var listOfAttributes = Grid.Children.OfType<ListBox>().ToList();
        Character_Refresh();
        foreach (var item in listOfAttributes) Grid.Children.Remove(item);
    }

    private void Open()
    {
        _status = Status.Open;
        var listOfAttributes = new ListBox();
        foreach (var item in Character.GetAttributes()) listOfAttributes.Items.Add(item);

        Height *= 2;
        listOfAttributes.Margin = new Thickness(
            Width * 0.05, Avatar.Height * 1.2, Width * 0.05, Height * 0.05
        );
        Grid.Children.Add(listOfAttributes);
    }

    private void AddAttribute(object sender, RoutedEventArgs e)
    {
        Close();
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.CreateList(false, true, Character);
    }

    public void Character_Refresh()
    {
        var listOfAttributes = Grid.Children.OfType<ListBox>().ToList();
        Character.Refrash(listOfAttributes[0].Items.OfType<IReturnValue>().ToList());
    }

    public void Refresh()
    {
        if (_status == Status.Open)
        {
            Character_Refresh();
            var lst = Grid.Children.OfType<ListBox>().ToList();
            foreach (var item in lst) Grid.Children.Remove(item);

            var listOfAttributes = new ListBox();
            foreach (var item in Character.GetAttributes()) listOfAttributes.Items.Add(item);

            listOfAttributes.Margin = new Thickness(
                Width * 0.05, Avatar.Height * 1.2, Width * 0.05, Height * 0.05
            );
        }
    }
}