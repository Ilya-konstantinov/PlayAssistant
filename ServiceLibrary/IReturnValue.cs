namespace ServiceLibrary;

public interface IReturnValue
{
    /// <summary>
    /// Название характеристики
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Ее значение
    /// </summary>
    string Value { get; set; }
}

public struct ReturnValue
{
    public ReturnValue(string title, string value)
    {
        Title = title;
        Value = value;
    }

    public string Title { get; set; }
    public string Value { get; set; }
}