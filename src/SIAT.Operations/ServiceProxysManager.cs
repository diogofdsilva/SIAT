using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using MS.OSM.Querys;
using SIAT.Service.Contract;
using SIAT.UserInfo.Contract;

namespace SIAT.Operations
{
    public class ServiceProxysManager
    {
        private readonly ServiceProxy<IOSMService> _osmService;
        private readonly ServiceProxy<ISIATService> _siatService;
        private readonly ServiceProxy<IUserInfo> _userInfoService;

        public ServiceProxysManager()
        {
            _osmService = new ServiceProxy<IOSMService>();
            _siatService = new ServiceProxy<ISIATService>();
            _userInfoService = new ServiceProxy<IUserInfo>();
        }

        public ServiceProxy<IOSMService> OSMService
        {
            get { return _osmService; }
        }

        public ServiceProxy<ISIATService> SIATService
        {
            get { return _siatService; }
        }

        public ServiceProxy<IUserInfo> UserInfoService
        {
            get { return _userInfoService; }
        }

        public void WakeUpAll()
        {
            _osmService.WakeUp();
            _siatService.WakeUp();
            _userInfoService.WakeUp();
        }

        public void UltimateWakeUpAll()
        {
            _osmService.UltimateWakeUp();
            _siatService.UltimateWakeUp();
            _userInfoService.UltimateWakeUp();
        }
    }

    public class ServiceProxy<T> where T : class
    {
        private T _serviceProxy;

        internal T Service
        {
            get
            {
                try
                {
                    return _serviceProxy;
                }
                catch (CommunicationException) //badabada
                {
                }
                catch (NullReferenceException)
                {
                }
                Discover();
                return Service;
            }
        }

        public ServiceProxy()
        {
            _serviceProxy = ServicesDiscovery.FindService<T>();
        }

        public ServiceProxy(Uri serviceUri)
        {
            Build(serviceUri);
        }

        public bool IsAlive()
        {
            if (_serviceProxy != null)
            {
                if (((ICommunicationObject)_serviceProxy).State != CommunicationState.Faulted)
                {
                    return true;
                }
            }

            return false;
        }

        public void Discover()
        {
            _serviceProxy = ServicesDiscovery.ExplicitFindService<T>();
            SaveConfig();
        }

        public void Build(Uri serviceUri)
        {
            _serviceProxy = ServicesDiscovery.BuildService<T>(serviceUri);
            SaveConfig();
        }

        public void Build()
        {
            string config = ConfigurationManager.AppSettings[typeof (T).Name + "Endpoint"];

            if (config == null)
            {
                return;
            }

            _serviceProxy = ServicesDiscovery.BuildService<T>(new Uri(config));
            SaveConfig();
        }

        public bool WakeUp()
        {
            EndpointAddress endpointAddress;

            if (_serviceProxy == null || (endpointAddress = ((IClientChannel)_serviceProxy).RemoteAddress) == null)
            {
                throw new EndpointNotFoundException("The endpoint was never found");
            }

            var webRequest = WebRequest.Create(endpointAddress.Uri);

            webRequest.Timeout = 5000;
            webRequest.Method = "GET";
            
            var webResponse = (HttpWebResponse) webRequest.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        private void SaveConfig()
        {
            if (_serviceProxy == null)
            {
                return;
            }

            var key = ((IClientChannel)_serviceProxy).RemoteAddress.Uri.ToString();
            ConfigurationManager.AppSettings[typeof (T).Name + "Endpoint"] = key;
        }

        public bool UltimateWakeUp()
        {
            try
            {
                WakeUp();                    
            }
            catch (EndpointNotFoundException)
            {
                Build();
                WakeUp();                    
            }

            return IsAlive();
        }
    }
}
