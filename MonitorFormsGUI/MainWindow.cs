using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using UsbMonitor;
using System.IO;
using System.Linq;
using MonitorFormsGUI.Properties;

namespace MonitorFormsGUI
{
    public partial class MainWindow : Form
    {
        private readonly UsbDriveMonitor _monitor;
        private readonly StorageManager<UsbDrive> _drivesStorage = new StorageManager<UsbDrive>();
        private readonly BindingList<UsbDrive> _drives;

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
            _drives = new BindingList<UsbDrive>(_monitor.AllDrives.Values.ToList());

            InitialiseGrid();

            _monitor.DriveArrived += NewDriveEventHandler;
        }

        private void InitialiseGrid()
        {
            monitoredDrivesView.DataSource = _drives;
            foreach (DataGridViewColumn column in monitoredDrivesView.Columns)
                column.HeaderText = GetTranslation(column.Name);
        }

        private static string GetTranslation(string key)
        {
            return Strings.ResourceManager.GetString(key) ?? key;
        }

        private void NewDriveEventHandler(object sender, DriveConnectedEventArgs e)
        {
            var addDrive = new Action(() => _drives.Add(e.Drive));
            if (monitoredDrivesView.InvokeRequired)
                monitoredDrivesView.Invoke(addDrive);
            else
                addDrive();
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
