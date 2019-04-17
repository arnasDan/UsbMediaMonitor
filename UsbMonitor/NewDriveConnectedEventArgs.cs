using System;

namespace UsbMonitor
{
    public class NewDriveConnectedEventArgs : EventArgs
    {
        public UsbDrive NewDrive { get; private set; }

        public NewDriveConnectedEventArgs(UsbDrive usbDrive)
        {
            NewDrive = usbDrive;
        }
    }
}
