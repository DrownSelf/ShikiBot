using System;

namespace Shiki
{
    class Programm
    {
        static void Main(string[] args)
        {
            new Ryougi().MainAsync().GetAwaiter().GetResult();
        }
    }
}