using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonitorFormsGUI.Annotations;
using MonitorFormsGUI.Properties;
using UsbMonitor;

namespace MonitorFormsGUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var trayApplication = new TrayApplication())
            {
                trayApplication.Run();
            }
        }
    }
}
