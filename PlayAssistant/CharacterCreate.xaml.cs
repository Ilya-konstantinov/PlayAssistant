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
        var lst = _character.GetAttributes();
        foreach (var attr in lst) Characteristic.Items.Add(attr);
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) item.IsEnabled = false;
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
            if (_character.Pic_path == "")
                _character.Pic_path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) +
                                      "\\avatar-example.png";

            parentWindow.AddCharacter(_character);
            parentWindow.RemoveCreateCharacter();
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
        foreach (var item in Characteristic.Items.OfType<UIElement>().ToList()) item.IsEnabled = false;
    }

    private void Avatar_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
        if (dialog.ShowDialog() == true) _character.Pic_path = dialog.FileName;
        Avatar.Background = new ImageBrush(new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute)));
    }
}