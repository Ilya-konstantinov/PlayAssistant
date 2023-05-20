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
    private readonly Character _character = new("");

    public CharacterCreate()
    {
        InitializeComponent();
        var lst = _character.GetAttributes();
        foreach (var attr in lst) Characteristic.Items.Add(attr);
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) { item.IsEnabled = false; }
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

            _character.Name = Name.Text;

            parentWindow.AddCharacter(_character);
            parentWindow.RemoveCreateCharacter();
        }
    }

    private void AddCharacteriscit_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.CreateList(false, false, _character);
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
        var lst = _character.GetAttributes();
        foreach (var attr in lst) Characteristic.Items.Add(attr);
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) { item.IsEnabled = false; }
    }
}