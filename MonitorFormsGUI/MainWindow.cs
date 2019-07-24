using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            ShowDebugElements();
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
            _monitor.DriveArrived += NewDriveEventHandler;
        }

        private void NewDriveEventHandler(object sender, DriveConnectedEventArgs e)
        {
            void AddDrive() => monitoredDrivesView.Rows.Add(e.Drive.Uuid);
            if (monitoredDrivesView.InvokeRequired)
                monitoredDrivesView.Invoke((Action) AddDrive);
            else
                AddDrive();
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

        private void AddDriveButton_Click(object sender, EventArgs e)
        {
            _monitor.SimulateDriveArrival("DEBUG_UUID");
        }

        [Conditional("DEBUG")]
        private void ShowDebugElements()
        {
            addDriveButton.Show();
        }
    }
}
