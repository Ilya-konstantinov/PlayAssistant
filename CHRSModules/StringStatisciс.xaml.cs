using System.Windows.Controls;
using ServiceLibrary;

namespace CHRSModules;

/// <summary>
///     Логика взаимодействия для StringStatiscic.xaml
/// </summary>
public partial class StringStatiscic : IReturnValue
{
    public StringStatiscic()
    {
        InitializeComponent();
        ElTitle.Content = "";
        Field.IsEnabled = false;
    }

    public StringStatiscic(string title = "", string value = "")
    {
        InitializeComponent();
        ElTitle.Content = title;
        Value = value;
        if (title == value) Field.IsEnabled = false;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => Field.Text;
        set => Field.Text = value;
    }

    private void Field_TextChanged(object sender, TextChangedEventArgs e)
    {
        Value = Field.Text;
    }
}