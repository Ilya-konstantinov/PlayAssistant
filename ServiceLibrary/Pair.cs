namespace ServiceLibrary;

public class Pair<T, TU>
{
    public Pair()
    {
    }

    public Pair(T first, TU second)
    {
        First = first;
        Second = second;
    }

    public T First { get; set; }
    public TU Second { get; set; }
}