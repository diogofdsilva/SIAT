using System.IO.IsolatedStorage;

namespace SIAT.PhoneApp
{
    public class ApplicationPhoneSettings
    {
        static ApplicationPhoneSettings()
        {
            SpeedMeasureType fromStorage;
            // Load PreventLock from Storage
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("SpeedMeasure", out fromStorage))
            {
                _speedMeasure = fromStorage;
            }
            else
            {
                SpeedMeasure = SpeedMeasureType.Mps;
            }

            bool gps;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("UseGPS", out gps))
            {
                _useGps = gps;
            }
            else
            {
                _useGps = false;
            }
        }

        private static SpeedMeasureType _speedMeasure;

        public static SpeedMeasureType SpeedMeasure 
        { 
            get
            {
                return _speedMeasure;
            }  
            set
            {
                _speedMeasure = value;
                SaveSetting("SpeedMeasure", _speedMeasure);
            }
        }

        private static bool _useGps;

        public static bool UseGPS
        {
            get
            {
                return _useGps;
            }
            set
            {
                _useGps = value;
                SaveSetting("UseGPS", _useGps);
            }
        }


        public static void SaveSetting(string key, object value)
        {

            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            else
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }

    public enum SpeedMeasureType
    {
        Kmh,Mph,Mps
    }
}