namespace HackerNewsProxy.Console.Helpers;

public static class ConsoleHelper
{
    public static void DisplayError(this Exception? e, int tabsCount = 0)
    {
        while (true)
        {
            if (e == null)
            {
                return;
            }

            $"{nameof(e.Message)}:".DisplayWithLeadingTabs(tabsCount);
            e.Message.DisplayWithLeadingTabs(tabsCount);

            if (string.IsNullOrEmpty(e.StackTrace) == false)
            {
                System.Console.WriteLine($"{nameof(e.StackTrace)}:");
                foreach (var line in e.StackTrace.Split(Environment.NewLine))
                {
                    line.DisplayWithLeadingTabs(tabsCount);
                }
            }

            e = e.InnerException;
            tabsCount += 1;
        }
    }

    public static void DisplayWithLeadingTabs(this string s, int tabsCount)
    {
        System.Console.WriteLine(new string('\t', tabsCount) + s);
    }
}