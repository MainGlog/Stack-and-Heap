using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Stack_and_Heap
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MyBenchmark>();
        }
    }
    
    [MemoryDiagnoser]
    public class MyBenchmark
    {
        private String DateAsText { get; } = "02 03 2025";
        [Benchmark]
        public (int month, int date, int year) DateWithSubstring()
        {
            // Each string is added to the heap
            String monthAsText = DateAsText.Substring(0, 2);
            String dateAsText = DateAsText.Substring(3, 2);
            String yearAsText = DateAsText.Substring(6);

            int month = int.Parse(monthAsText);
            int date = int.Parse(dateAsText);
            int year = int.Parse(yearAsText);

            return (month, date, year);
        }
        [Benchmark]
        public (int month, int date, int year) DateWithSpan()
        {
            ReadOnlySpan<char> dateAsSpan = DateAsText;
            // Readonly Span is a structure, meaning it will go in the stack instead of the heap

            ReadOnlySpan<char> monthAsText = dateAsSpan.Slice(0, 2);
            ReadOnlySpan<char> dateAsText = dateAsSpan.Slice(3, 2);
            ReadOnlySpan<char> yearAsText = dateAsSpan.Slice(6);

            int month = int.Parse(monthAsText);
            int date = int.Parse(dateAsText);
            int year = int.Parse(yearAsText);

            return (month, date, year);
        }
    }
        
}
