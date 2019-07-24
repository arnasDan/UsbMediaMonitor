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
        [JsonProperty]
        public string VolumeName { get; internal set; }
        [JsonProperty]
        public string DriveLetter { get; internal set; }
        public string Uuid { get; }
        public bool Monitored { get; set; }
        [JsonIgnore]
        public bool IsProcessRunning { get; private set; }
        public string ConsoleCommand
        {
            get => _command;

            set
            {
                lock (_commandLock)
                {
                    _command = value;
                    if (!IsProcessRunning && _process != null)
                    {
                        _process.Dispose();
                        _process.Exited -= ExitedProcessHandler;
                    }
                    _process = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            //TODO: this is commented out to simplify testing: WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = "/C " + _command
                        },
                        EnableRaisingEvents = true
                    };
                    _process.Exited += ExitedProcessHandler;
                }
            }
        }

        private string _command;

        private Process _process;

        private readonly object _commandLock = new object();

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
            
            lock (_commandLock)
            {
                IsProcessRunning = true;
                _process?.Start();
            }
        }

        private void ExitedProcessHandler(object sender, EventArgs e)
        {
            var senderProcess = (Process) sender;
            lock(_commandLock)
                if (_process != senderProcess)
                {
                    senderProcess.Exited -= ExitedProcessHandler;
                    senderProcess.Dispose();
                }
            IsProcessRunning = false;
        }

    }
}
