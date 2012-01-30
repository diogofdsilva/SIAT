using System.Windows.Media;

namespace SIAT.PhoneApp.ViewModels.Models
{
    public class IntensityColor
    {
        public static Color GetIntensityColor(int intensity)
        {
            switch (intensity)
            {
                case 1:
                    return Colors.Green;
                case 2:
                    return Colors.Green;
                case 3:
                    return Colors.Green;
                case 4:
                    return Colors.Yellow;
                case 5:
                    return Colors.Yellow;
                case 7:
                    return Colors.Orange;
                case 8:
                    return Colors.Orange;
                case 9:
                    return Colors.Red;
                case 10:
                    return Colors.Red;
                    
            }

            return Colors.White;
        }

    }
}
