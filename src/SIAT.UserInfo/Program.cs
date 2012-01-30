using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SIAT.UserInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t ***************************************************");
            Console.WriteLine("\n\t *               UserInfo - Service                *\n");
            Console.WriteLine("\t ***************************************************\n");
            using (ServiceHost host = new ServiceHost(typeof(UserInfoService)))
            {
                Console.WriteLine("-> Initiating UserInfo Service");
                host.Open();
                Console.WriteLine("-> Press Any key to EXIT");
                Console.Read();

                host.Close();
            }
        }
    }
}
