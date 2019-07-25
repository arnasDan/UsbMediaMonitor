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
    //TODO: refresh more often - extend UsbDrive with INotifyModified?
    public partial class MainWindow : Form
    {
        private readonly UsbDriveMonitor _monitor;
        private readonly StorageManager<UsbDrive> _drivesStorage = new StorageManager<UsbDrive>();
        private readonly BindingList<UsbDrive> _drives;
        private bool _requiresSave = false;

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
                MessageBox.Show(Strings.CannotReadDriveFile + exception.Message);
            }

            _monitor = new UsbDriveMonitor(drives);
            _drives = new BindingList<UsbDrive>(_monitor.AllDrives.Values.ToList());

            InitializeGrid();
            LocalizeControls();

            MaximizeBox = false;
            ShowIcon = false;

            _monitor.DriveArrived += NewDriveEventHandler;
        }

        private void InitializeGrid()
        {
            monitoredDrivesView.DataSource = _drives;
            monitoredDrivesView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            foreach (DataGridViewColumn column in monitoredDrivesView.Columns)
            {
                column.HeaderText = GetTranslation(column.Name);
                switch (column.Name)
                {
                    case "Uuid":
                        column.Width = 225;
                        break;
                    case "ConsoleCommand":
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                    default:
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;
                }
            }


            var openFileColumn = new DataGridViewButtonColumn()
            {
                Name = "OpenFile",
                Text = GetTranslation("OpenFile"),
                HeaderText = "",
                UseColumnTextForButtonValue = true
            };
            monitoredDrivesView.Columns.Add(openFileColumn);

            monitoredDrivesView.CellContentClick += MonitoredDrivesView_CellContentClick;
        }

        private void LocalizeControls()
        {
            foreach (Control control in Controls)
                control.Text = GetTranslation(control.Text);
        }

        private static string GetTranslation(string key)
        {
            return Strings.ResourceManager.GetString(key) ?? key;
        }

        private void NewDriveEventHandler(object sender, DriveConnectedEventArgs e)
        {
            var addDrive = new Action(() =>
            {
                if (_drives.All(d => d.Uuid != e.Drive.Uuid))
                    _drives.Add(e.Drive);
                else
                    _drives.ResetBindings();
            });
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
                MessageBox.Show(Strings.SavingError + exception.Message);
            }
            MessageBox.Show(Strings.DriveListSaved);
        }

        private void AddDriveButton_Click(object sender, EventArgs e)
        {
            var uuid = randomDriveCheckbox.Checked ? Guid.NewGuid().ToString().ToUpper() : "DEBUG_UUID";
            _monitor.SimulateDriveArrival(uuid);
        }

        private void MonitoredDrivesView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _drives[e.RowIndex].ConsoleCommand = $"\"{openFileDialog.FileName}\"";
                    monitoredDrivesView.Update();
                    monitoredDrivesView.Refresh();
                }
            }
        }

        [Conditional("DEBUG")]
        private void ShowDebugElements()
        {
            addDriveButton.Show();
            randomDriveCheckbox.Show();
        }
    }
}
