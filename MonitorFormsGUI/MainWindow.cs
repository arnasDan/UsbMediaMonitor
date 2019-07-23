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
using System.IO;
using Newtonsoft.Json;

namespace MonitorFormsGUI
{
    public partial class MainWindow : Form
    {
        private readonly UsbDriveMonitor _monitor;
        private readonly JsonSerializer _serializer = new JsonSerializer()
        {
            Formatting = Formatting.Indented
        };

        public MainWindow()
        {
            InitializeComponent();
            IEnumerable<UsbDrive> drives = null;

            using (var reader = new StreamReader("drives.json"))
            using (var jsonReader = new JsonTextReader(reader))
            {
                try
                {
                    drives = _serializer.Deserialize<IEnumerable<UsbDrive>>(jsonReader);
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot read drive file!");
                }
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StreamWriter("drives.json"))
                {
                    _serializer.Serialize(writer, _monitor.MonitoredDrives);
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show("An error occured while saving: " + exception.Message);
            }
            MessageBox.Show("Drive list saved succesfully!");
        }
    }
}
