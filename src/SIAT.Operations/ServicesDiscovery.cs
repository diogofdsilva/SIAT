using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Text;

namespace SIAT.Operations
{
    class ServicesDiscovery
    {
        public static T FindService<T>()
        {
            Binding binding = new BasicHttpBinding();
            Type type = typeof(T);
            
            var criteria = new FindCriteria(type);
            criteria.Duration = TimeSpan.FromSeconds(2);
            criteria.MaxResults = 1;

            var client = new DiscoveryClient(new UdpDiscoveryEndpoint());
            try
            {
                Collection<EndpointDiscoveryMetadata> services = client.Find(criteria).Endpoints;

                if (services.Count == 0)
                {
                    Console.WriteLine("\n {0} - Not found.", type.Name);
                    return default(T);
                }
                else
                {
                    Console.WriteLine("\n {0} - Found.", type.Name);
                    return ChannelFactory<T>.CreateChannel(binding, services[0].Address);
                }
            }
            catch(SocketException)
            {
                Console.WriteLine("\n {0} - Not found - Error.", type.Name);
                return default(T);
            }
        }


        public static T ExplicitFindService<T>()
        {
            Binding binding = new BasicHttpBinding();
            Type type = typeof(T);

            var criteria = new FindCriteria(type);
            criteria.Duration = TimeSpan.FromSeconds(2);
            criteria.MaxResults = 1;

            var client = new DiscoveryClient(new UdpDiscoveryEndpoint());
            Collection<EndpointDiscoveryMetadata> services = client.Find(criteria).Endpoints;

            if (services.Count == 0)
            {
                throw new EndpointNotFoundException("None endpoint found for service " + type.Name);
            }
            
            return ChannelFactory<T>.CreateChannel(binding, services[0].Address);
        }

        public static T BuildService<T>(Uri serviceUri)
        {
            Binding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress(serviceUri);

            return ChannelFactory<T>.CreateChannel(binding,address);
        }
    }
}
