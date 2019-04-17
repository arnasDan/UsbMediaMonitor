using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace UsbMonitor
{
    public class UsbDriveMonitor
    {
        private readonly ManagementEventWatcher watcher = new ManagementEventWatcher();
        public ConcurrentDictionary<string, UsbDrive> AllDrives { get; }
        public event EventHandler<NewDriveConnectedEventArgs> NewDriveArrived;

        public IEnumerable<UsbDrive> MonitoredDrives { get => AllDrives.Values.Where(drive => drive.Monitor); }
        
        public UsbDriveMonitor(IEnumerable<UsbDrive> usbDrives = null)
        {
            if (usbDrives == null)
                AllDrives = new ConcurrentDictionary<string, UsbDrive>();
            else
                AllDrives = new ConcurrentDictionary<string, UsbDrive>(usbDrives.ToDictionary(drive => drive.Uuid));
            
            watcher.Query = new WqlEventQuery("SELECT * FROM WIN32_VolumeChangeEvent WHERE EventType = 2");
            watcher.EventArrived += VolumeChangeHandler;
            watcher.Start();
        }

        private void VolumeChangeHandler(object sender, EventArrivedEventArgs e)
        {
            string volume = e.NewEvent.GetPropertyValue("DriveName").ToString();
            string uuid = e.NewEvent.GetQualifierValue("UUID").ToString();
            string volumeLabel = new DriveInfo(volume.Substring(0, 1)).VolumeLabel;
            UsbDrive connectedDrive;

            Debug.WriteLine($"Drive connected. UUID={uuid}, volume={volume}, volumeLabel={volumeLabel}");
            if (AllDrives.TryGetValue(uuid, out connectedDrive))
            {
                Debug.WriteLine("Drive already known");
                connectedDrive.DriveLetter = volume;
                connectedDrive.Name = volumeLabel;
                if (connectedDrive.Monitor)
                    connectedDrive.ExecuteCommand();
                
            }
            else
            {
                Debug.WriteLine("Drive is previously unknown");
                connectedDrive = new UsbDrive(uuid)
                {
                    DriveLetter = volume,
                    Name = volumeLabel,
                    //DEBUG DATA
                    ConsoleCommand = "pause",
                    Monitor = true
                };
                AllDrives[uuid] = connectedDrive;
            }
            NewDriveArrived?.Invoke(this, new NewDriveConnectedEventArgs(connectedDrive));
        }
    }
}
