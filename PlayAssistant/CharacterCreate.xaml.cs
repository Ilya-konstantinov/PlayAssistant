using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ServiceLibrary;

/*
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ServiceLibrary; 
*/

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для CharacterCreate.xaml
/// </summary>
public partial class CharacterCreate : UserControl
{
    private readonly Character _character = new("", "");

    public CharacterCreate()
    {
        InitializeComponent();
        var attributes = _character.GetAttributes();
        foreach (var attribute in attributes) Characteristic.Items.Add(attribute);
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) item.IsEnabled = false;
    }

    public bool NameIsCorrect()
    {
        return Name.Text.Length > 0;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (NameIsCorrect())
        {
            var parentWindow = Window.GetWindow(this) as MainWindow;

            _character.Name = Name.Text;
            if (_character.AvatarPath == "")
                _character.AvatarPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) +
                                      "\\avatar-example.png";

            parentWindow.AddCharacter(_character);
            parentWindow.RemoveCharacterCreationPanel();
        }
    }

    private void AddCharacteristic_Click(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.CreateList(false, false, _character);
    }

    public void AddCharacter(IReturnValue chr)
    {
        Characteristic.Items.Add(chr);
    }

    private void CloseClick(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this) as MainWindow;
        parentWindow.RemoveCharacterCreationPanel();
    }

    public void Refresh()
    {
        Characteristic.Items.Clear();
        var listOfAttributes = _character.GetAttributes();
        foreach (var attr in listOfAttributes) Characteristic.Items.Add(attr);
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) item.IsEnabled = false;
    }

    private void Avatar_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
        if (dialog.ShowDialog() == true) _character.AvatarPath = dialog.FileName;
        Avatar.Background = new ImageBrush(new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute)));
    }
}