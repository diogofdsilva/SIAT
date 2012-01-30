using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SIAT.PhoneApp.Controller;

namespace SIAT.PhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        private MainController _controller;
        private bool _started;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            _controller = new MainController(this.Dispatcher);
            this.DataContext = _controller.Model;
            this.OccurrencesLst.ItemsSource = _controller.Model.LocationList;
            //this.map.DataContext = _controller.Model.LocationList;
            //this.OccurrencesLst.DataContext = _controller.Model;
            //this.ListOfItems.DataContext = _controller.Model;
            _started = false;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var firstAppBarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            
            //this._controller.Model.LocationList.Add(new LocationData(new GeoCoordinate(38,-9), 
             //   new SolidColorBrush(Colors.Orange), "teste" ));a

            

            if (!_started)
            {
                if (!ApplicationPhoneSettings.UseGPS)
                {
                    MessageBox.Show("This features need to use GPS functionality. Please enable it on Settings Panel.");
                    return;
                }

                firstAppBarButton.Text = "stop";
                firstAppBarButton.IconUri = new Uri("/icons/appbar.transport.pause.rest.png", UriKind.Relative);
                _controller.StartTracking();
                _started = true;
            }
            else
            {
                _controller.StopTracking();
                _started = false;
                firstAppBarButton.Text = "start";
                firstAppBarButton.IconUri = new Uri("/icons/appbar.transport.play.rest.png", UriKind.Relative);
            }
        }
        
        private void SettingsMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void StartMenuItem_Click(object sender, EventArgs e)
        {
            this.Button_Click(sender,null);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }


        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sourceTB = (TextBlock) sender;


            string uri = string.Format("/OcurrenceDetails.xaml?occurrenceId={0}", sourceTB.Tag);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }


    
}