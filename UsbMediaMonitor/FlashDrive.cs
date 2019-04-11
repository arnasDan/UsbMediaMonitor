using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbMediaMonitor
{
    class FlashDrive
    {
        public string Name { get; set; }
        public string DriveLetter { get; set; }
        public string ConsoleCommand
        {
            get => command;
            
            set
            {
                lock (commandLock)
                {
                    command = value;
                    process = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            //WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = "/C" + command
                        }
                    };
                }
                    
            }
            
        }

        private string command;

        private Process process;

        private readonly object commandLock = new object();

        public void ExecuteCommand()
        {
            lock (commandLock)
                process.Start();
        }

    }
}
