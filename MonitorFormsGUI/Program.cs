using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonitorFormsGUI.Annotations;
using MonitorFormsGUI.Properties;

namespace MonitorFormsGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow window = null;
            using (var icon = new NotifyIcon()
            {
                Icon = System.Drawing.SystemIcons.Information,
                Visible = true
            })
            {
                void WindowShow(object s, EventArgs e)
                {
                    if (window == null || window.IsDisposed)
                        window = new MainWindow();
                    window.Show();
                }
                icon.ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem(Strings.Show, WindowShow),
                    new MenuItem(Strings.Exit, (s, _) => Application.Exit())
                });
                icon.Click += WindowShow;
                Application.Run();
                icon.Visible = false;
            }
        }
    }
}
