using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace SIAT.PhoneApp.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private bool _milesPerHour, _kmsPerHour;

        public SettingsViewModel()
        {
            switch (ApplicationPhoneSettings.SpeedMeasure)
            {
                case SpeedMeasureType.Kmh:
                    _kmsPerHour = true;
                    break;
                case SpeedMeasureType.Mph:
                    _milesPerHour = true;
                    break;
                
            }
        }

        public bool PreventLock
        {
            get
            {
                return ApplicationIdleModeHelper.Current.PreventLock;
            }
            set
            {
                ApplicationIdleModeHelper.Current.PreventLock = value;
                NotifyPropertyChanged("PreventLock");
            }
        }

        public bool UseGPS
        {
            get
            {
                return ApplicationPhoneSettings.UseGPS;
            }
            set
            {
                ApplicationPhoneSettings.UseGPS = value;
                NotifyPropertyChanged("UseGPS");
            }
        }
        
        public bool RunsUnderLock
        {
            get
            {
                return ApplicationIdleModeHelper.Current.RunsUnderLock;
            }
            set
            {

                ApplicationIdleModeHelper.Current.RunsUnderLock = value;
                NotifyPropertyChanged("RunsUnderLock");
            }
        }

        public bool MilesPerHour
        {
            get { return _milesPerHour; }
            set 
            { 
                _milesPerHour = value;
                ApplicationPhoneSettings.SpeedMeasure = SpeedMeasureType.Mph;
                NotifyPropertyChanged("MilesPerHour"); 
            }
        }

        public bool KmsPerHour
        {
            get { return _kmsPerHour; }
            set
            {
                _kmsPerHour = value;
                ApplicationPhoneSettings.SpeedMeasure = SpeedMeasureType.Kmh;
                NotifyPropertyChanged("KmsPerHour");
            }
        }
        
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}