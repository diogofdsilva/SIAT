using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using Microsoft.Maps.MapControl;

namespace SIAT.SilverlightApplication
{
    /// <summary>
    /// This class contains an embedded Location object. It acts as DataContext
    /// for the individual items within MapItemsControl. Thus {Binding Location}
    /// translates to LocationData.Location
    /// </summary>
    public class OccurrenceData
    {
        public Location Location
        {
            get; set;
        }

        public Brush Color
        {
            get; set;
        }

        public string WayName
        {
            get;
            set;
        }

        public OccurrenceData()
        {
            this.Location = new Location();
        }
    }

    /// <summary>
    /// This class exposes IEnumerable, and acts as ItemsSource for 
    /// MapItemsControl
    /// </summary>
    public class OccurrenceDataCollection : ObservableCollection<OccurrenceData>
    {
        
        public OccurrenceDataCollection()
        {
        }

        public void Load()
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(mvc_OpenReadCompleted);

            Uri baseUri = new Uri(webClient.BaseAddress);
            UriBuilder uriBuilder = new UriBuilder(baseUri.Scheme, baseUri.Host, baseUri.Port);
            uriBuilder.Path = "/Occurrence/Occurrences";

            webClient.OpenReadAsync(uriBuilder.Uri);
        }

        private void mvc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if(e.Error != null)
                return;

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<Occurrence>));
            List<Occurrence> data = (List<Occurrence>)json.ReadObject(e.Result);

            foreach (var occurrence in data)
            {
                this.Add(new OccurrenceData()
                             {
                                 Location =
                                     new Location(Convert.ToDouble(occurrence.Latitude),
                                                  Convert.ToDouble(occurrence.Longitude)),
                                 Color = new SolidColorBrush(Colors.Red),
                                 WayName = occurrence.WayName
                             });
            }
        }
    }
}
