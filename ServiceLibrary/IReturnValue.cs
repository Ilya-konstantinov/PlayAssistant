namespace ServiceLibrary;

public interface IReturnValue
{
    string Title { get; set; }

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