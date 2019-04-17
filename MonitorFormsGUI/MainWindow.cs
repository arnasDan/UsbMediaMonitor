using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbMonitor;

namespace MonitorFormsGUI
{
    public partial class MainWindow : Form
    {
        private UsbDriveMonitor monitor = new UsbDriveMonitor();
        public MainWindow()
        {
            InitializeComponent();
            monitor.NewDriveArrived += NewDriveEventHandler;
        }

        private void NewDriveEventHandler(object sender, NewDriveConnectedEventArgs e)
        {
            Action action = () => monitoredDrivesView.Rows.Add(e.NewDrive.Uuid);
            if (monitoredDrivesView.InvokeRequired)
                monitoredDrivesView.Invoke(action);
            else
                action();
        }
    }
}
