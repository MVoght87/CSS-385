#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MichaelVoght_NameSpace
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MichaelVoght
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new ExtremePaddle())
                game.Run();
        }
    }
#endif
}
