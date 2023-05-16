namespace ServiceLibrary
{
    public interface IReturnValue
    {
        string Title { get; set; }

        string Value { get; set; }
    }
    public struct ReturnValue
    {
        public ReturnValue(string _Title, string _Value) { Title = _Title; Value = _Value; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
