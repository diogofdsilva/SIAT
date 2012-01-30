using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Threading.Tasks;
using SIAT.Operations.Messages;
using SIAT.WP7.Shared.DAL;

namespace SIAT.WP7.NotificationService
{
    public class OcurrenceMessageHandler
    {
        private MessageQueue _messageQueue;
        private string _messageQueueName = @".\private$\OccurrencesUpdate_WP7";

        private NotificationSenderUtility _pushNotificationUtils;

        public OcurrenceMessageHandler()
        {
            if (!MessageQueue.Exists(_messageQueueName))
            {
                _messageQueue = MessageQueue.Create(_messageQueueName);
            }
            else
            {
                _messageQueue = new MessageQueue(_messageQueueName);    
            }

            _pushNotificationUtils = new NotificationSenderUtility();
            _messageQueue.ReceiveCompleted += OnNewOccurrenceMessage;
            _messageQueue.BeginReceive();
        }

        private void OnNewOccurrenceMessage(object sender, ReceiveCompletedEventArgs e)
        {
            Console.WriteLine("New Message");

            Message message = _messageQueue.EndReceive(e.AsyncResult);
            NewOccurrenceMessage newOccurrenceMessage = (NewOccurrenceMessage) message.Body;

            Task.Factory.StartNew(() => OnNewOccurrence(newOccurrenceMessage));

            _messageQueue.BeginReceive();
        }


        private void OnNewOccurrence(NewOccurrenceMessage message)
        {
            List<Uri> listUri;
            using (WP7InfoDataAccessLayer context = new WP7InfoDataAccessLayer())
            {
                listUri = context.Subcriptions.GetAllFromWay(message.WayId).Select(s => new Uri(s.Uri)).ToList();   
            }

            _pushNotificationUtils.SendRawNotification(listUri, BitConverter.GetBytes(message.WayId), assyncCallback);
        }

        private static void assyncCallback(CallbackArgs response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.NotificationStatus);    
            }
        }
    }
}