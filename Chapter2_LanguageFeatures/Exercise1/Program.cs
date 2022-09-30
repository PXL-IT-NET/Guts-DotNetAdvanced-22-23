namespace Exercise1;

public class Program
{
    public static void Main(string[] args)
    {
        CompositionSearcher searcher = new CompositionSearcher();

        //Filter posts by using QuickSearch
        Console.Write("Enter search keyword for quick search: ");
        ReadInputAndDoSearch(searcher, CompositionFilters.QuickFilter);
        
        //Filter posts by using DetailedSearch
        Console.Write("Enter search keyword for detailed search: ");
        ReadInputAndDoSearch(searcher, CompositionFilters.DetailedFilter);
        
        //Filter posts by using ReleaseYearSearch
        Console.Write("Enter year to search the music: ");
        ReadInputAndDoSearch(searcher, CompositionFilters.ReleaseYearFilter);
        
        Console.ReadLine();
    }

    private static void ReadInputAndDoSearch(CompositionSearcher searcher, CompositionFilterDelegate filterMethod)
    {
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            IList<Composition> result = searcher.SearchMusic(filterMethod, input);
            DisplayMusicDetails(result);
        }
        else
        {
            Console.Write("Invalid input");
        }
        Console.Write("");
    }

    private static void DisplayMusicDetails(IList<Composition> compositions)
    {
        if (compositions.Count == 0)
        {
            Console.WriteLine("No compositions found");
            Console.WriteLine("-----------------------------------------------");
            return;
        }

        foreach (var composition in compositions)
        {
            Console.WriteLine(composition);
            Console.WriteLine("-----------------------------------------------");
        }
    }
    
  
}