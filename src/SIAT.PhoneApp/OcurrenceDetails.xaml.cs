using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SIAT.PhoneApp.ViewModels;

namespace SIAT.PhoneApp
{
    public partial class OcurrenceDetails : PhoneApplicationPage
    {


        public OcurrenceDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string idQueryString = NavigationContext.QueryString["occurrenceId"];
            int id;

            if (Int32.TryParse(idQueryString, out id))
            {
                var occurrence = MainViewModel.Current.LocationList.SingleOrDefault(o => o.Id == id);

                this.DataContext = occurrence;    
            }

            base.OnNavigatedTo(e);
        }
    }
}