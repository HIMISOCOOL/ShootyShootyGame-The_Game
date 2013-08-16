using System;

namespace shipgame_windows
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RunGame game = new RunGame())
            {
                game.Run();
            }
        }
    }
#endif
}

