using System;

namespace SIAT.WP7.NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = new OcurrenceMessageHandler();
            Console.WriteLine("WP7 Notification Service - Waiting!");
            Console.Read();
        }
    }
}
