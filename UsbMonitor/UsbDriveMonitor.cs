using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace UsbMonitor
{
    public class UsbDriveMonitor
    {
        private readonly ManagementEventWatcher _watcher = new ManagementEventWatcher();
        public ConcurrentDictionary<string, UsbDrive> AllDrives { get; }
        public event EventHandler<DriveConnectedEventArgs> DriveArrived;

        public IEnumerable<UsbDrive> MonitoredDrives => AllDrives.Values.Where(drive => drive.Monitored);

        public UsbDriveMonitor(IEnumerable<UsbDrive> usbDrives = null)
        {
            usbDrives = usbDrives ?? Enumerable.Empty<UsbDrive>();
            AllDrives = new ConcurrentDictionary<string, UsbDrive>(usbDrives.ToDictionary(drive => drive.Uuid));
            
            _watcher.Query = new WqlEventQuery("SELECT * FROM WIN32_VolumeChangeEvent WHERE EventType = 2");
            _watcher.EventArrived += VolumeChangeHandler;
            _watcher.Start();
        }

        private void VolumeChangeHandler(object sender, EventArrivedEventArgs e)
        {
            var volume = e.NewEvent.GetPropertyValue("DriveName").ToString();
            var uuid = e.NewEvent.GetQualifierValue("UUID").ToString();
            var volumeLabel = new DriveInfo(volume.Substring(0, 1)).VolumeLabel;

            Debug.WriteLine($"Drive connected. UUID={uuid}, volume={volume}, volumeLabel={volumeLabel}");
            var drive = AllDrives.AddOrUpdate(
                uuid,
                new UsbDrive(uuid)
                {
                    DriveLetter = volume,
                    VolumeName = volumeLabel
                },
                (key, connectedDrive) =>
                {
                    Debug.WriteLine("Drive already known");
                    connectedDrive.DriveLetter = volume;
                    connectedDrive.VolumeName = volumeLabel;
                    if (connectedDrive.Monitored)
                        connectedDrive.ExecuteCommand();
                    return connectedDrive;
                }
            );
            DriveArrived?.Invoke(this, new DriveConnectedEventArgs(drive));
        }

        [Conditional("DEBUG")]
        public void SimulateDriveArrival(string uuid)
        {
            if (AllDrives.TryGetValue(uuid, out var existingDrive))
            {
                if (existingDrive.Monitored)
                    existingDrive.ExecuteCommand();
                return;
            }
            var drive = AllDrives.AddOrUpdate(
                uuid,
                new UsbDrive(uuid)
                {
                    Monitored = true,
                    VolumeName = "test",
                    ConsoleCommand = "explorer.exe",
                    DriveLetter = "X"
                },
                (key, connectedDrive) =>
                {
                    Debug.WriteLine("Drive already known");
                    connectedDrive.DriveLetter = "X";
                    connectedDrive.VolumeName = "test";
                    if (connectedDrive.Monitored)
                        connectedDrive.ExecuteCommand();
                    return connectedDrive;
                }
            );

            DriveArrived?.Invoke(this, new DriveConnectedEventArgs(drive));
        }
    }
}
