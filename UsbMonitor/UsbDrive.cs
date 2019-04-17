using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UsbMonitor
{
    public class UsbDrive
    {
        public string Name { get; set; }
        public string DriveLetter { get; set; }
        public string Uuid { get; }
        public bool Monitor { get; set; }
        [JsonIgnore]
        public bool IsProcessRunning { get; private set; }
        public string ConsoleCommand
        {
            get => command;

            set
            {
                lock (commandLock)
                {
                    command = value;
                    if (!IsProcessRunning && process != null)
                    {
                        process.Dispose();
                        process.Exited -= ExitedProcessHandler;
                    }
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
                    process.Exited += ExitedProcessHandler;
                }
            }
        }

        private string command;

        private Process process;

        private readonly object commandLock = new object();

        public UsbDrive(string uuid)
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
            
            lock (commandLock)
            {
                IsProcessRunning = true;
                process?.Start();
            }
        }

        private void ExitedProcessHandler(object sender, EventArgs e)
        {
            var senderProcess = sender as Process;                                                                                                                                                                  
            lock(commandLock)
                if (process != senderProcess)
                {
                    senderProcess.Exited -= ExitedProcessHandler;
                    senderProcess.Dispose();
                }
            IsProcessRunning = false;
        }

    }
}
