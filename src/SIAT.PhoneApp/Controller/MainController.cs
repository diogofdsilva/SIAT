using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SIAT.PhoneApp.Position;
using SIAT.PhoneApp.SIATServiceReference;
using SIAT.PhoneApp.ViewModels;
using SIAT.PhoneApp.ViewModels.Models;

namespace SIAT.PhoneApp.Controller
{
    public class MainController
    {
        private MainViewModel _model;
        private PhonePosition _position;
        private SIATServiceWP7Client _siatServicesClient;
        private Way _currentWay;
        private Timer _refreshWayTimer;
        private bool _isStarted;
        private Dispatcher _mainDispatcher;

        public MainController(Dispatcher dispatcher)
        {
            _mainDispatcher = dispatcher;
            _model = MainViewModel.Current;
            _position = new PhonePosition(_model);

            // Timer
            _isStarted = false;
            _refreshWayTimer = new Timer(RefreshData, this, 1000, 30000);

            //Services Client initiation
            InitializeServiceClient();
        }

        private void InitializeServiceClient()
        {
            _siatServicesClient = new SIATServiceWP7Client();
            _siatServicesClient.OpenCompleted += _siatServicesClient_OpenCloseCompleted;
            _siatServicesClient.CloseCompleted += _siatServicesClient_OpenCloseCompleted;
            _siatServicesClient.ListAllOccurrencesCompleted += _siatServicesClient_ListAllOccurrencesCompleted;
            _siatServicesClient.ListAllOccurrencesInWayCompleted += _siatServicesClient_ListAllOccurrencesInWayCompleted; 
            _siatServicesClient.AlertCompleted += _siatServicesClient_AlertCompleted;
            _siatServicesClient.CurrentWayCompleted += _siatServicesClient_CurrentWayCompleted;
            _siatServicesClient.SendPositionInformationCompleted += _siatServicesClient_OpenCloseCompleted;
        }

        public MainViewModel Model
        {
            get { return _model; }
        }

        private void RefreshData(object state)
        {
            if (!_isStarted)
            {
                return;
            }

            RefreshWay();
            RefreshLocationList();
            SendPhonePositionInfo();
        }

        #region SendDigest

        private void SendPhonePositionInfo()
        {
            
            var observCollection = new ObservableCollection<PositionInfo>();
            
            _position.GetLastPositions().ForEach(o => observCollection.Add(new PositionInfo()
                                                                  {
                                                                      
                                                                      Lat = o.Location.Latitude,        
                                                                      Lon = o.Location.Longitude,
                                                                      Speed = o.Location.Speed,
                                                                      Date = 
                                                                          o.Timestamp.ToUniversalTime().UtcDateTime

                                                                  }));
            _siatServicesClient.SendPositionInformationAsync(observCollection); 
        }
        #endregion SendDigest

        #region CurrentWay

        private void RefreshWay()
        {
            if (_model.GeoCoordinate == null)
            {
                return;
            }

            _siatServicesClient.CurrentWayAsync(_model.GeoCoordinate.Latitude, _model.GeoCoordinate.Longitude);
        }

        private void _siatServicesClient_CurrentWayCompleted(object sender, CurrentWayCompletedEventArgs e)
        {
            RefreshConnectionStatus();

            if (e.Error != null || e.Result == null)
            {
                return;
            }

            _currentWay = e.Result;

            if (!String.IsNullOrEmpty(e.Result._ref))
            {
                _model.WayName = e.Result._ref;
            }
            else if (!String.IsNullOrEmpty(e.Result._name))
            {
                _model.WayName = e.Result._name;
            }
            else
            {
                _model.WayName = e.Result._type;
            }

            
        }

        #endregion CurrentWay

        #region Connection

        void _siatServicesClient_OpenCloseCompleted(object sender, AsyncCompletedEventArgs e)
        {
            RefreshConnectionStatus();
        }
        
        private void RefreshConnectionStatus()
        {
            if (!_mainDispatcher.CheckAccess())
            {
                _mainDispatcher.BeginInvoke(RefreshConnectionStatus);
                return;
            }

            switch (_siatServicesClient.State)
            {
                case CommunicationState.Closed:
                    _model.ConnectionStatus = new SolidColorBrush(Colors.Red);
                    break;
                case CommunicationState.Opened:
                    _model.ConnectionStatus = new SolidColorBrush(Colors.Green);
                    break;
                default:
                    _model.ConnectionStatus = new SolidColorBrush(Colors.Orange);
                    break;
            }
        }

        #endregion Connection

        #region ListOcurrences

        private void RefreshLocationList()
        {
            //
            // If there is no current way or the actual way is in a residential zone, get all the ocurrences. 
            // Otherwise, get only the occurrence in the current way.
            //
            if (_currentWay == null || _currentWay._type == "residential")
            {
                _siatServicesClient.ListAllOccurrencesAsync();    
            }
            else
            {
                _siatServicesClient.ListAllOccurrencesInWayAsync(_currentWay._id);
            }
            
        }

        private void _siatServicesClient_ListAllOccurrencesCompleted(object sender, ListAllOccurrencesCompletedEventArgs e)
        {
            RefreshConnectionStatus();
            if (e.Error != null)
            {
                return;
            }

            RefreshLocationList(e.Result);
        }

        private void _siatServicesClient_ListAllOccurrencesInWayCompleted(object sender, ListAllOccurrencesInWayCompletedEventArgs e)
        {
            RefreshConnectionStatus();
            if (e.Error != null)
            {
                return;
            }

            RefreshLocationList(e.Result);
        }

        private void RefreshLocationList(Collection<Occurrence> result)
        {

            //_model.LocationList.Load(result);
            _mainDispatcher.BeginInvoke(() =>
                {
                    _model.LocationList.Clear();

                    if (result.Count == 0)
                    {
                        _model.IsRoadClear = Visibility.Visible;
                    }
                    else
                    {
                        foreach (var d in result)
                        {
                            var currentLocation = new LocationData
                                (
                                new GeoCoordinate(d._lat, d._lon),
                                new SolidColorBrush(IntensityColor.GetIntensityColor(d._intensity)),
                                d._wayName
                                );
                            currentLocation.Id = d._id;
                            currentLocation.Intensity = d._intensity;
                            currentLocation.Latitude = d._lat;
                            currentLocation.Longitude = d._lon;

                            _model.LocationList.Add(currentLocation);
                        }
                        _model.IsRoadClear = Visibility.Collapsed;
                    }
                });
            
        }

        #endregion ListOcurrences

        #region Alert

        public void SendAlert()
        {
            if (!_isStarted)
            {
                return;
            }
            
            var alert = new Alert
                            {
                                _lat = _model.GeoCoordinate.Latitude,              
                                _lon = _model.GeoCoordinate.Longitude,
                                _level = AlertLevel.RoadBlock
                            };

            _siatServicesClient.AlertAsync(alert);
        }

        private void _siatServicesClient_AlertCompleted(object sender, AsyncCompletedEventArgs e)
        {
            RefreshConnectionStatus();
        }

        #endregion Alert

        #region Stop/Start

        public void StartTracking()
        {
            _isStarted = true;
            _position.StartPositionWatch();
            RefreshData(null);
        }

        public void StopTracking()
        {
            _isStarted = false;
            _position.StopPositionWatch();
            _siatServicesClient.CloseAsync();
        }

        #endregion Stop/Start

    }
}
