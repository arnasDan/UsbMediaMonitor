using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbMediaMonitor
{
    public class FlashDrive
    {
        public string Name { get; set; }
        public string DriveLetter { get; set; }
        public string Uuid { get; }
        public bool Monitor { get; set; }
        public bool IsProcessRunning { get; private set; }

        public string ConsoleCommand
        {
            get => command;
            
            set
            {
                lock (commandLock)
                {
                    command = value;
                    process?.Dispose();
                    process = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            //commented out to simplify testing WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = "/C " + command
                        },
                        EnableRaisingEvents = true
                    };
                    process.Exited += ExitedProcessExitedEventHandler;
                }      
            }         
        }

        private string command;

        private Process process;

        private readonly object commandLock = new object();

        public FlashDrive(string uuid)
        {
            Uuid = uuid;
        }

        public void ExecuteCommand()
        {
            if (IsProcessRunning)
            {
                Debug.WriteLine("Skipping execution, process is already running");
                return;
            }
            Debug.WriteLine($"Executing command: {ConsoleCommand}");
            IsProcessRunning = true;
            lock (commandLock)
                process.Start();
        }

        private void ExitedProcessExitedEventHandler(object sender, EventArgs e)
        {
            var senderProcess = sender as Process;
            lock(commandLock)
                if (process != senderProcess)
                    senderProcess.Exited -= ExitedProcessExitedEventHandler;
            IsProcessRunning = false;
        }

    }
}
