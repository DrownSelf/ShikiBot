using System.Threading.Tasks;

namespace Shiki
{
    class Context
    {
        public delegate Task PlayerMode();
        public enum HandleMode
        {
            Resume = 0,
            Pause = 1,
            Stop = 2
        }
    }
}
