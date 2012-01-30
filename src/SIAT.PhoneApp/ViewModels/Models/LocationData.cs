using System.Device.Location;
using System.Windows.Media;

namespace SIAT.PhoneApp.ViewModels.Models
{
    /// <summary>
    /// This class contains an embedded Location object. It acts as DataContext
    /// for the individual items within MapItemsControl. Thus {Binding Location}
    /// translates to LocationData.Location
    /// </summary>
    public class LocationData
    {
        public int Id
        {
            get;
            set;
        }

        public GeoCoordinate Location
        {
            get;
            set;
        }

        public Brush Color
        {
            get;
            set;
        }

        public string WayName
        {
            get;
            set;
        }

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }

        public int Intensity
        {
            get;
            set;
        }

        public LocationData(GeoCoordinate location, Brush color, string wayName)
        {
            Location = location;
            
            Color = color;
            WayName = wayName;
        }

        public LocationData()
        {
            this.Location = new GeoCoordinate();
            this.WayName = string.Empty;
            this.Color = new SolidColorBrush(Colors.Transparent);
        }
    }
}