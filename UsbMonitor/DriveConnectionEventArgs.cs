using System;

namespace UsbMonitor
{
    public class DriveConnectionEventArgs : EventArgs
    {
        public UsbDrive Drive { get; }

        public DriveConnectionEventArgs(UsbDrive drive)
        {
            Drive = drive;
        }
    }
}
