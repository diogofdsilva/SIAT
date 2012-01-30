using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace SIAT.Monitor
{
    public class ProcessData : INotifyPropertyChanged
    {
        public Process Process
        {
            get;
            set;
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _output;

        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                NotifyPropertyChanged("Output");
            }
        }

        private string _input;

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                NotifyPropertyChanged("Input");
            }
        }

        public ProcessData(ProcessStartInfo processStartInfo)
        {
            Input = string.Empty;

            Process = new Process();
            Process.StartInfo = processStartInfo;

            Process.OutputDataReceived += POutputDataReceived;
            Process.ErrorDataReceived += POutputDataReceived;
            
            Process.EnableRaisingEvents = true;
            Process.Exited += Process_Exited;

            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();

            Title = this.Process.ProcessName;

            
        }

        void Process_Exited(object sender, EventArgs e)
        {
            Output += "\n\t ---> FIM DA EXECUÇÃO <---";
        }

        void POutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data + "\n";
        }

        public void SendInput()
        {
            Output += string.Format("|-> {0} \n", Input);
            this.Process.StandardInput.Write(Input);
            this.Process.StandardInput.Flush();
            this.Process.Refresh();
            
            Input = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class ProcessDataCollection : ObservableCollection<ProcessData>
    {
        public ProcessDataCollection()
        {
        }
    }
   
}