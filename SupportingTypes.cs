namespace Sorting_algorithms;

public class AlgorithmInfo
{
    public string Name { get; set; }
    public Func<int[], IEnumerable<SortStep>> SortMethod { get; set; }
    public string Note { get; set; } = "";
}
public enum SortType { Begin, Compare, Swap, Done }

public class SortStep
{
    public int[] Array { get; set; }
    public SortType SortType { get; set; }
    public int? IndexA { get; set; }
    public int? IndexB { get; set; }
}