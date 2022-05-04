using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;

namespace FPS_Launcher
{
    enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rootPath;
        private string versionFile;
        private string gameZip;
        private string gameExe;

        private LauncherStatus _status;
        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        PlayButton.Content = "Graj";
                        break;
                    case LauncherStatus.failed:
                        PlayButton.Content = "Download Failed - Retry";
                        break;
                    case LauncherStatus.downloadingGame:
                        PlayButton.Content = "Downloading Game";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        PlayButton.Content = "Downloading Update";
                        break;
                    default:
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            rootPath = Directory.GetCurrentDirectory();
            versionFile = Path.Combine(rootPath, "Version.txt");
            gameZip = /*Path.Combine(rootPath, "Build.zip");*/ rootPath + "/Build.zip";
            gameExe = Path.Combine(rootPath, "Build", "FPS.exe");
        }

        private void CheckForUpdates()
        {
            if (File.Exists(versionFile))
            {
                Version localVersion = new Version(File.ReadAllText(versionFile));
                VersionText.Content = localVersion.ToString();

                try
                {
                    WebClient webClient = new WebClient();
                    Version onlineVersion = new Version(webClient.DownloadString("https://drive.google.com/uc?export=download&id=18Zp8ka-HAxmaDAO8uSDo19vVPjYuET87"));

                    if (onlineVersion.IsDifferentVersionThan(localVersion))
                    {
                        InstallGameFiles(false, onlineVersion);
                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                    }
                }
                catch (Exception e)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Pojawił się problem w trakcje sprawdzanie aktualizacji gry!\n{e.Message}","ERROR",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            else
            {
                InstallGameFiles(true, Version.zero);
            }
        }

        private void InstallGameFiles(bool _isUpdate, Version _onlineVersion)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    if (_isUpdate)
                    {
                        Status = LauncherStatus.downloadingUpdate;
                    }
                    else
                    {
                        Status = LauncherStatus.downloadingGame;
                        _onlineVersion = new Version(webClient.DownloadString("https://drive.google.com/uc?export=download&id=18Zp8ka-HAxmaDAO8uSDo19vVPjYuET87"));
                    }

                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                    webClient.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1KearEDJ9IidoZRSEiFZ3zVYTfuF1qQbR"), gameZip, _onlineVersion);
                }

                //ZipFile.ExtractToDirectory(gameZip, rootPath);
            }
            catch (Exception e)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Pojawił się problem w trakcje pobierania plików gry!\n{e.Message}","ERROR",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();
                ZipFile.ExtractToDirectory(gameZip, rootPath, true);
                File.Delete(gameZip);

                File.WriteAllText(versionFile, onlineVersion);

                VersionText.Content = onlineVersion;
                Status = LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Pojawił się problem w trakcje wyodrębniania plików gry!\n{ex.Message}\n{ex.Source}","ERROR",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(gameExe) && Status == LauncherStatus.ready)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(gameExe);
                processStartInfo.WorkingDirectory = Path.Combine(rootPath, "Build");
                Process.Start(processStartInfo);

                Close();
            }
            else if (Status == LauncherStatus.failed)
            {
                CheckForUpdates();
            }
        }

        private void OnWindowRendered(object sender, EventArgs e)
        {
            CheckForUpdates();
        }
    }

    struct Version
    {
        internal static Version zero = new Version(0, 0, 0);
        private short major;
        private short minor;
        private short patch;

        public Version(short major, short minor, short patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }
        internal Version(string _version)
        {
            string[] _versionStrings = _version.Split('.');
            if (_versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                patch = 0;
                return;
            }

            major = short.Parse(_versionStrings[0]);
            minor = short.Parse(_versionStrings[1]);
            patch = short.Parse(_versionStrings[2]);
        }

        internal bool IsDifferentVersionThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (patch != _otherVersion.patch)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return major + "." + minor + "." + patch;
        }
    }
}
