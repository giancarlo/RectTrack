using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RectTrackTest
{
    static class Program
    {
        static Form1 mainform;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainform = new Form1();
            Application.Run(mainform);
        }
    }
}
