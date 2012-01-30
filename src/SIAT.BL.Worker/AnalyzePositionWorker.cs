using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using SIAT.Operations;

namespace SIAT.Worker
{
    class AnalyzePositionWorker
    {
        private const string AnalyzePositionQueuePath = @".\private$\AnalyzePositionsWorker";
        private readonly MessageQueue _messageQueue;
        private readonly SIATOperations _operations;
        
        public AnalyzePositionWorker()
        {
            _operations = new SIATOperations();

            try
            {
                _messageQueue = new MessageQueue(AnalyzePositionQueuePath);
                
            }
            catch (MessageQueueException e)
            {
                if (MessageQueue.Exists(AnalyzePositionQueuePath))
                {
                    return;
                }
                _messageQueue = MessageQueue.Create(AnalyzePositionQueuePath);
            }

            _messageQueue.ReceiveCompleted += OnNewMessageReceived;
            _messageQueue.BeginReceive();
        }

        private void OnNewMessageReceived(object sender, ReceiveCompletedEventArgs e)
        {
            Console.WriteLine("New Message");

            Message message = _messageQueue.EndReceive(e.AsyncResult);
            List<PositionInfo> listPositionInfo = (List<PositionInfo>)message.Body;

            Task.Factory.StartNew(() => _operations.AnalyzePositionsDigest(listPositionInfo));
            _messageQueue.BeginReceive();
        }
    }
}
