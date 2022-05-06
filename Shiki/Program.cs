using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DSharpPlus;
using Shiki.Commands;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;

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