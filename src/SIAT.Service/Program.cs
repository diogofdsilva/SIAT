using System;
using System.ServiceModel;

namespace SIAT.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t ***************************************************");
            Console.WriteLine("\n\t *                 SIAT - Service                  *\n");
            Console.WriteLine("\t ***************************************************\n");
            using (ServiceHost host = new ServiceHost(typeof(SIATService)))
            {
                Console.WriteLine("-> Initiating SIAT Service");
                host.Open();
                Console.WriteLine("-> Press Any key to EXIT");
                Console.Read();

                host.Close();
            }
        }
    }
}
