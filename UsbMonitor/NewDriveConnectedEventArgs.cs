using System;

namespace UsbMonitor
{
    public class DriveConnectedEventArgs : EventArgs
    {
        public UsbDrive Drive { get; }

        public DriveConnectedEventArgs(UsbDrive drive)
        {
            Drive = drive;
        }
    }
}
