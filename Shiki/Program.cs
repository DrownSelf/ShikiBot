namespace Shiki
{
    class Program
    {
        static void Main(string[] args)
        {
            new ShikiBot().MainAsync().GetAwaiter().GetResult();
        }
    }
}