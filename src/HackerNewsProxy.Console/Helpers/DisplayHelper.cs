namespace HackerNewsProxy.Console.Helpers;

public static class DisplayHelper
{
    public static void DisplayCollection<T>(this IEnumerable<T> collection)
    {
        System.Console.WriteLine("[" + string.Join(", ", collection) + "]");
    }
}