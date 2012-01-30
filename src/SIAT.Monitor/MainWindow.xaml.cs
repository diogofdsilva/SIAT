using System;

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SIAT.Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private const string siatPath = @"\SIATService\SIAT.Service.exe";
        private const string osmPath = @"\OSMService\SIAT.OSM.Service.exe";
        private const string workerPath = @"\Worker\SIAT.BL.Worker.exe";
        private const string userPath = @"\UserInfoService\SIAT.UserInfo.exe";
        private const string wp7NotifPath = @"\WP7Notification\SIAT.WP7.NotificationService.exe";
        private string _buildPath;

        private ProcessDataCollection processCollection;
        
        public MainWindow()
        {
            InitializeComponent();
            _buildPath = System.Configuration.ConfigurationManager.AppSettings["BuildRootPath"];

            processCollection = new ProcessDataCollection();
            Tab.ItemsSource = processCollection;
        }

        private void siatLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LauchProcess(siatPath);
        }

        private void osmLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LauchProcess(osmPath);
        }

        private void workerLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LauchProcess(workerPath);
        }

        private void wp7NotificationLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LauchProcess(wp7NotifPath);
        }

        private void userLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LauchProcess(userPath);
        }

        private void closeTab(object sender, RoutedEventArgs e)
        {
            ProcessData processData = (ProcessData) ((Button) sender).CommandParameter;

            processData.Process.Close();
            processData.Process.Dispose();
            processCollection.Remove(processData);
        }

        private void sendInput(object sender, RoutedEventArgs e)
        {
            ProcessData processData = (ProcessData)((Button)sender).CommandParameter;

            processData.SendInput();
        }

        private Process LauchProcess(string path)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(_buildPath + path);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.UseShellExecute = false;

            ProcessData processData = new ProcessData(processStartInfo);
            processCollection.Add(processData);


            return processData.Process;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (ProcessData processData in processCollection)
            {
                processData.Process.Close();
                processData.Process.Dispose();
            }
        }

        #endregion

        
    }
}
