using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UsbMonitor;
using System.IO;

namespace MonitorFormsGUI
{
    public partial class MainWindow : Form
    {
        private readonly UsbDriveMonitor _monitor;
        private readonly StorageManager<UsbDrive> _drivesStorage = new StorageManager<UsbDrive>();

        public MainWindow()
        {
            InitializeComponent();
            IEnumerable<UsbDrive> drives = null;

            try
            {
                drives = _drivesStorage.Read();
            }
            catch (IOException exception)
            {
                MessageBox.Show("Cannot read drive file: " + exception.Message);
            }

            _monitor = new UsbDriveMonitor(drives);
            _monitor.NewDriveArrived += NewDriveEventHandler;
        }

        private void NewDriveEventHandler(object sender, DriveConnectedEventArgs e)
        {
            void Action() => monitoredDrivesView.Rows.Add(e.Drive.Uuid);
            if (monitoredDrivesView.InvokeRequired)
                monitoredDrivesView.Invoke((Action) Action);
            else
                Action();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                _drivesStorage.Save(_monitor.MonitoredDrives);
            }
            catch (IOException exception)
            {
                MessageBox.Show("An error occured while saving: " + exception.Message);
            }
            MessageBox.Show("Drive list saved succesfully!");
        }
    }
}
