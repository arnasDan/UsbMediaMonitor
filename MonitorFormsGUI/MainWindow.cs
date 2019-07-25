using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using UsbMonitor;
using System.IO;
using System.Linq;
using MonitorFormsGUI.Properties;
using System.Drawing;

namespace MonitorFormsGUI
{
    public partial class MainWindow : Form
    {
        private readonly BindingList<UsbDrive> _drives;
        private readonly UsbDriveMonitor _monitor;
        private readonly StorageManager<UsbDrive> _drivesStorage;

        private bool RequiresSave
        {
            get => saveRequiredLabel.Visible;
            set => saveRequiredLabel.Visible = value;
        }

        public MainWindow(UsbDriveMonitor monitor, StorageManager<UsbDrive> drivesStorage)
        {
            InitializeComponent();
            ShowDebugElements();

            _monitor = monitor;
            _drivesStorage = drivesStorage;

            _drives = new BindingList<UsbDrive>(_monitor.AllDrives.Values.ToList());

            InitializeGrid();
            LocalizeControls();

            MaximizeBox = false;
            ShowIcon = false;

            _monitor.DriveArrived += NewDriveEventHandler;
            _monitor.DriveRemoved += DisconnectedDriveEventHandler;

            FormClosing += (s, _) =>
            {
                if (!RequiresSave) return;
                var result = MessageBox.Show(Strings.SaveUnsaved, "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Save();
            };
        }

        private void InitializeGrid()
        {
            monitoredDrivesView.RowHeadersVisible = false;
            monitoredDrivesView.AllowUserToResizeRows = false;
            monitoredDrivesView.BackgroundColor = Color.White;
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
            monitoredDrivesView.CellValueChanged += (s, _) => { RequiresSave = true; };
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

        private void NewDriveEventHandler(object sender, DriveConnectionEventArgs e)
        {
            var addDrive = new Action(() =>
            {
                if (_drives.All(d => d.Uuid != e.Drive.Uuid))
                    _drives.Add(e.Drive);
                else
                {
                    monitoredDrivesView.Update();
                    monitoredDrivesView.Refresh();
                }
            });
            if (monitoredDrivesView.InvokeRequired)
                monitoredDrivesView.Invoke(addDrive);
            else
                addDrive();
        }
        private void DisconnectedDriveEventHandler(object sender, DriveConnectionEventArgs e)
        {
            var removeDrive = new Action(() => _drives.Remove(e.Drive));
            if (!e.Drive.Monitored)
            {
                if (monitoredDrivesView.InvokeRequired)
                    monitoredDrivesView.Invoke(removeDrive);
                else
                    removeDrive();
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        private void Save(bool showSuccessMessage = false)
        {
            try
            {
                _drivesStorage.Save(_monitor.MonitoredDrives);
                RequiresSave = false;
                if (showSuccessMessage)
                    MessageBox.Show(Strings.DriveListSaved);
            }
            catch (IOException exception)
            {
                MessageBox.Show(Strings.SavingError + exception.Message);
            }
        }

        private void AddDriveButton_Click(object sender, EventArgs e)
        {
            var uuid = randomDriveCheckbox.Checked ? Guid.NewGuid().ToString().ToUpper() : "DEBUG_UUID";
            _monitor.SimulateDriveArrival(uuid);
        }

        private void MonitoredDrivesView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;
            if (!(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn) || e.RowIndex < 0) return;

            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                _drives[e.RowIndex].ConsoleCommand = $"\"{openFileDialog.FileName}\"";
                RequiresSave = true;
                monitoredDrivesView.Update();
                monitoredDrivesView.Refresh();
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
