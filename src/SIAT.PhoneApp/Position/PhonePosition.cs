using System.Linq;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows.Media;
using SIAT.PhoneApp.ViewModels;

namespace SIAT.PhoneApp.Position
{

    public class PhonePosition
    {
        private GeoCoordinateWatcher _watcher;
        private Stack<GeoPosition<GeoCoordinate>> _positions;
        private MainViewModel _mainViewModel;
 
        public PhonePosition(MainViewModel mainViewModel) {
            _positions = new Stack<GeoPosition<GeoCoordinate>>();
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _watcher.PositionChanged += watcher_PositionChanged;
            _watcher.StatusChanged += watcher_StatusChanged;
            _mainViewModel = mainViewModel;
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Ready:
                    _mainViewModel.GPSStatus = new SolidColorBrush(Colors.Green);
                    break;
                    
                default:
                    _mainViewModel.GPSStatus = new SolidColorBrush(Colors.Red);
                    break;
            }
        }

        public void StartPositionWatch() 
        {
            _watcher.Start();
        }

        public void StopPositionWatch()
        {
            _watcher.Stop();
        }

        public GeoPosition<GeoCoordinate> LastPosition
        {
            get
            {
                return _positions.Peek();
            }
        }

        public GeoPositionStatus CurrentStatus
        {
            get
            {
                return _watcher.Status;
            }
        }

        public List<GeoPosition<GeoCoordinate>> GetLastPositions()
        {
            var positions = _positions.ToList();
            positions.Clear();
            return positions;
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            _positions.Push(e.Position);
            _mainViewModel.GeoCoordinate = e.Position.Location;
        }

        
    }
}
