using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace UsbMediaMonitor
{
    public class UsbMonitor
    {
        private readonly ManagementEventWatcher watcher = new ManagementEventWatcher();
        public ConcurrentDictionary<string, FlashDrive> AllDrives { get; } = new ConcurrentDictionary<string, FlashDrive>();

        public IEnumerable<FlashDrive> MonitoredDrives { get => AllDrives.Values.Where(drive => drive.Monitor); }
        
        public UsbMonitor()
        {
            watcher.Query = new WqlEventQuery("SELECT * FROM WIN32_VolumeChangeEvent WHERE EventType = 2");
            watcher.EventArrived += VolumeChangeHandler;
            watcher.Start();
        }

        private void VolumeChangeHandler(object sender, EventArrivedEventArgs e)
        {
            var volume = e.NewEvent.GetPropertyValue("DriveName").ToString();
            var uuid = e.NewEvent.GetQualifierValue("UUID").ToString();
            var volumeLabel = new DriveInfo(volume.Substring(0, 1)).VolumeLabel;

            Debug.WriteLine($"Drive connected. UUID={uuid}, volume={volume}, volumeLabel={volumeLabel}");
            if (AllDrives.TryGetValue(uuid, out var drive))
            {
                Debug.WriteLine("Drive already known");
                drive.DriveLetter = volume;
                drive.Name = volumeLabel;
                if (drive.Monitor)
                    drive.ExecuteCommand();
            }
            else
            {
                Debug.WriteLine("Drive is previously unknown");
                AllDrives[uuid] = new FlashDrive(uuid)
                {
                    DriveLetter = volume,
                    Name = volumeLabel,
                    //DEBUG DATA
                    ConsoleCommand = "cmd.exe",
                    Monitor = true
                };
            }
            
        }
    }
}
