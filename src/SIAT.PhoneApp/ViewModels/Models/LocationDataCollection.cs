using System.Collections.ObjectModel;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SIAT.PhoneApp.SIATServiceReference;

namespace SIAT.PhoneApp.ViewModels.Models
{
    /// <summary>
    /// This class exposes IEnumerable, and acts as ItemsSource for 
    /// MapItemsControl
    /// </summary>
    public class LocationDataCollection : ObservableCollection<LocationData>
    {
        
    }
}