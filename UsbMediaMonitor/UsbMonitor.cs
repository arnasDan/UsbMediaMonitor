using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;

namespace UsbMediaMonitor
{
    public class UsbMonitor
    {
        private readonly ManagementEventWatcher watcher = new ManagementEventWatcher();
        
        
        public UsbMonitor()
        {
            watcher.Query = new WqlEventQuery("SELECT * FROM WIN32_VolumeChangeEvent WHERE EventType = 2");
            watcher.EventArrived += VolumeChangeHandler;
            watcher.Start();
        }

        private void VolumeChangeHandler(object sender, EventArrivedEventArgs e)
        {
            var volume = e.NewEvent.Properties["DriveName"].Value.ToString().Substring(0, 1);
            Console.WriteLine(volume);
            var flashDrive = new DriveInfo(volume);
            Console.WriteLine(flashDrive.VolumeLabel);
        }
    }
}
