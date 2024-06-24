namespace Superkatten.Katministratie.Host.Pages.Reports;

public static class ReadOnlyCollectionIndexOfExtention
{
    public static int IndexOf<T>(this IReadOnlyCollection<T> self, Func<T, bool> predicate)
    {
        for (int i = 0; i < self.Count; i++)
        {
            if (predicate(self.ElementAt(i)))
            {
                return i;
            }
        }

        return -1;
    }
}
