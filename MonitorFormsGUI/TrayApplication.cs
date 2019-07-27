
using System;
using System.IO;
using System.Windows.Forms;
using MonitorFormsGUI.Properties;
using UsbMonitor;

namespace MonitorFormsGUI
{
    public class TrayApplication : IDisposable
    {
        private bool _disposed = false;
        private MainWindow _window = null;
        private readonly UsbDriveMonitor _monitor;
        private readonly StorageManager<UsbDrive> _drivesStorage = new StorageManager<UsbDrive>();

        private readonly NotifyIcon icon = new NotifyIcon
        {
            Icon = System.Drawing.SystemIcons.Information
        };

        public TrayApplication()
        {
            icon.ContextMenu = new ContextMenu(new[]
            {
                new MenuItem(Strings.Show, (s, _) => ShowWindow()),
                new MenuItem(Strings.Exit, (s, _) => Application.Exit())
            });
            icon.Click += (s, __) => ShowWindow();

            try
            {
                _monitor = new UsbDriveMonitor(_drivesStorage.Read());

            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
            {
                MessageBox.Show(Strings.CannotReadDriveFile + exception.Message);
            }
        }

        public void Run()
        {
            icon.Visible = true;
            Application.Run();
            icon.Visible = false; 
        }

        private void ShowWindow()
        {
            if (_window == null || _window.IsDisposed)
                _window = new MainWindow(_monitor, _drivesStorage);
            _window.Show();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _window?.Dispose();
                icon.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
