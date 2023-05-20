using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ServiceLibrary;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для CharacterCreate.xaml
/// </summary>
public partial class CharacterCreate : UserControl
{
    private readonly Character character = new("");

    public CharacterCreate()
    {
        InitializeComponent();
        Refrash();
    }

    public bool NameCorrect()
    {
        if (Name.Text.Length > 0)
            return true;
        return false;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (NameCorrect())
        {
            var parentWindow = Window.GetWindow(this) as MainWindow;

            character.Name = Name.Text;

            parentWindow.AddCharacter(character);
            parentWindow.RemoveCreateCharacter();
        }
    }

    private void AddCharacteriscit_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.CreateList(false, false, character);
    }

    public void AddCharacter(IReturnValue chr)
    {
        Characteristic.Items.Add(chr);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.RemoveCreateCharacter();
    }

    public void Refrash()
    {
        Characteristic.Items.Clear();
        var lst = character.GetAttributes();
        foreach (var attr in lst) Characteristic.Items.Add(attr);


        var t = Characteristic.Items.OfType<UIElement>().ToList();
        foreach (var attr in t)
            attr.IsEnabled = false;
    }
}