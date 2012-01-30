using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Phone.Notification;
using System.Device.Location;
using SIAT.PhoneApp.SIATServiceReference;
using SIAT.PhoneApp.ViewModels;

namespace SIAT.PhoneApp.PushNotifications
{
    public class PushNotificationRegister
    {

        private const string CHANNEL_NAME = "SIATClientNotificationChannel";
        private HttpNotificationChannel _phoneNotificationChannel;
        private SIATServiceWP7Client _client;
        private MainViewModel _model;
        private Uri _currentUri;
        

        public PushNotificationRegister(SIATServiceWP7Client serviceWP7Client, MainViewModel model) 
        {
            this._client = serviceWP7Client;
            this._model = model;

            _client.CreateOrUpdateOccurrencesSubscriptionCompleted += _client_CreateOrUpdateOccurrencesSubscriptionCompleted;
            _client.DeleteOccurrencesSubscriptionCompleted += _client_DeleteOccurrencesSubscriptionCompleted;
            _client.ChangeOccurrencesSubscriptionUriCompleted += _client_ChangeOccurrencesSubscriptionUriCompleted;
            
            //
            // Procura se já existe um canal aberto
            //
            
            _phoneNotificationChannel = HttpNotificationChannel.Find(CHANNEL_NAME);

            if (_phoneNotificationChannel == null)
            {
                _phoneNotificationChannel = new HttpNotificationChannel(CHANNEL_NAME);
            }

            _phoneNotificationChannel.ChannelUriUpdated += PhoneNotificationChannelChannelUriUpdated;
            _phoneNotificationChannel.HttpNotificationReceived += phoneNotificationChannel_HttpNotificationReceived;

            _phoneNotificationChannel.Open();
        }

        void phoneNotificationChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            //
            // Parse Content 
            // New Occurrences
            //

            using (var reader = new StreamReader(e.Notification.Body))
            {
                string payload = reader.ReadToEnd();
                
            }
        }


        void PhoneNotificationChannelChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            //
            // Actualizar URI no Serviço
            //
            _client.ChangeOccurrencesSubscriptionUriAsync(_currentUri,e.ChannelUri);
            _currentUri = e.ChannelUri;
        }

        void _client_ChangeOccurrencesSubscriptionUriCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
        }
        
        public void CreateOrUpdateSubsciption(long wayId)
        {
            _client.CreateOrUpdateOccurrencesSubscriptionAsync(_currentUri, wayId);
        }
        
        void _client_CreateOrUpdateOccurrencesSubscriptionCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

        }

        public void DropSubsciption()
        {
            _client.DeleteOccurrencesSubscriptionAsync(_currentUri);
        }

        void _client_DeleteOccurrencesSubscriptionCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                return;
            }

        }

    }
}
