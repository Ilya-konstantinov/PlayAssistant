using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

public partial class CharacterForList : UserControl
{
    public readonly Character Character;
    private Status _st = Status.Close;

    internal CharacterForList(Character character)
    {
        InitializeComponent();
        Character = character;
        Name.Content = character.Name;
    }

    private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_st == Status.Open)
        {
            _st = Status.Close;
            var lst = Grid.Children.OfType<ListBox>().ToList();
            Height /= 2;
            Character.Refrash(lst[0].Items.OfType<IReturnValue>().ToList());
            foreach (var item in lst) Grid.Children.Remove(item);
        }
        else
        {
            _st = Status.Open;
            var lst = new ListBox();
            foreach (var item in Character.GetAttributes()) lst.Items.Add(item);

            Height *= 2;
            lst.Margin = new Thickness(
                Width * 0.05, Avatar.Height * 1.2, Width * 0.05, Height * 0.05
            );
            Grid.Children.Add(lst);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.CreateList(false, true, Character);
    }

    public void Refresh()
    {
        if (_st == Status.Open)
        {
            var lst = Grid.Children.OfType<ListBox>().ToList();
            foreach (var item in lst) Grid.Children.Remove(item);

            var lstt = new ListBox();
            foreach (var item in Character.GetAttributes()) lstt.Items.Add(item);

            lstt.Margin = new Thickness(
                Width * 0.05, Avatar.Height * 1.2, Width * 0.05, Height * 0.05
            );
        }
    }
}