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
        private UsbDriveMonitor monitor;
        private JsonSerializer serializer = new JsonSerializer()
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
                    drives = serializer.Deserialize<IEnumerable<UsbDrive>>(jsonReader);
                }
                catch (IOException)
                {
                    MessageBox.Show("Cannot read drive file!");
                }
            }
            
            monitor = new UsbDriveMonitor(drives);
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StreamWriter("drives.json"))
                {
                    serializer.Serialize(writer, monitor.MonitoredDrives);
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
