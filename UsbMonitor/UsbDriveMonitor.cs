﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

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
            AllDrives.AddOrUpdate(
                uuid,
                new UsbDrive(uuid)
                {
                    DriveLetter = volume,
                    Name = volumeLabel,
                    //TODO: Remove this, DEBUG DATA
                    ConsoleCommand = "pause",
                    Monitored = true
                },
                (key, connectedDrive) =>
                {
                    Debug.WriteLine("Drive already known");
                    connectedDrive.DriveLetter = volume;
                    connectedDrive.Name = volumeLabel;
                    if (connectedDrive.Monitored)
                        connectedDrive.ExecuteCommand();
                    return connectedDrive;
                }
            );
            DriveArrived?.Invoke(this, new DriveConnectedEventArgs(AllDrives[uuid]));
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
            var drive = new UsbDrive(uuid)
            {
                Monitored = true,
                Name = "test",
                ConsoleCommand = "pause",
                DriveLetter = "X"
            };
            AllDrives[uuid] = drive;

            DriveArrived?.Invoke(this, new DriveConnectedEventArgs(drive));
        }
    }
}
