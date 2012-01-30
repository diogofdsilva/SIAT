using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;
using SIAT.PhoneApp.Position;
using SIAT.PhoneApp.SIATServiceReference;
using SIAT.PhoneApp.ViewModels.Models;

namespace SIAT.PhoneApp.ViewModels 
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private LocationDataCollection _locationList;
        
        private Brush _GPSStatus;
        private Brush _connectionStatus;
        private GeoCoordinate _geoCoordinate;
        private string _wayName;
        private Visibility _roadClear;


        private MainViewModel()
        {
            _GPSStatus = _connectionStatus = new SolidColorBrush(Colors.Red);
            _locationList = new LocationDataCollection();
            _list = new List<LocationData>();
            _roadClear = Visibility.Visible;
        }

        public GeoCoordinate GeoCoordinate
        {
            get { return _geoCoordinate; }
            set
            {
                _geoCoordinate = value;
                NotifyPropertyChanged("GeoCoordinate");
                NotifyPropertyChanged("Speed");
            }
        }

        #region SpeedUnitsFactors

        private const double MilesPerHourFactor = 2.2369;
        private const double KmsPerHourFactor = 3.6;

        #endregion SpeedUnitsFactors

        public string Speed
        {
            get
            {
                try
                {

                    if(this.GeoCoordinate != null && !double.IsNaN(this.GeoCoordinate.Speed))
                    {
                        switch (ApplicationPhoneSettings.SpeedMeasure)
                        {
                            case SpeedMeasureType.Kmh:
                                return String.Format("{0:0.00} {1}", this.GeoCoordinate.Speed * KmsPerHourFactor, "km/h");
                            case SpeedMeasureType.Mph:
                                return String.Format("{0:0.00} {1}", this.GeoCoordinate.Speed * MilesPerHourFactor, "mph");
                            case SpeedMeasureType.Mps:
                                return String.Format("{0:0.00} {1}", this.GeoCoordinate.Speed, "m/s");
                            default:
                                return String.Format("{0:0.00} {1}", this.GeoCoordinate.Speed, "m/s");
                        }
                    }
                }
                catch
                {
                }
                return "N/A";
            }
        }

        public string WayName
        {
            get
            {
                if (_wayName == null)
                {
                    return "N/A";
                }
                return _wayName;
            }
            set 
            {
                _wayName = value;
                NotifyPropertyChanged("WayName");
            }
        }

        public Visibility IsRoadClear
        {
            get
            {
                return _roadClear;
            }
            set
            {
                _roadClear = value;
                NotifyPropertyChanged("IsRoadClear");
            }
        }

        public Brush GPSStatus
        {
            get { return _GPSStatus; }
            set
            {
                _GPSStatus = value;
                NotifyPropertyChanged("GPSStatus");
            }
        }

        public Brush ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                NotifyPropertyChanged("ConnectionStatus");
            }
        }

        
        public LocationDataCollection LocationList
        {
            get
            {
                return _locationList;

            }
            set
            {
                _locationList = value;
            }
        }


        private List<LocationData> _list;

        public List<LocationData> Ocurrences
        {
            get
            {
                NotifyPropertyChanged("Ocurrences");
                return _list;

            }
            set { _list = value; }
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

        #region Static

        private static MainViewModel _currentModel;
        

        public static MainViewModel Current
        {
            get
            {
                if (_currentModel == null)
                {
                    _currentModel = new MainViewModel();
                }
                return _currentModel;
            }
        }

        #endregion Static
    }
}
