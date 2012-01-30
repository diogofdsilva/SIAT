using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using MS.OSM.Querys.DTO;
using SIAT.Operations;
using SIAT.Service.Contract.DTO;
using SIAT.Services.Contract;

namespace SIAT.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public abstract class SIATServices : ISIATServices
    {
        
        public SIATServices()
        {
            _siatOperations = new SIATOperations();
            _siatOperations.ProxysManager.UltimateWakeUpAll();
        }

        #region Implementation of ISIATServices

        private SIATOperations _siatOperations;

        [OperationBehavior(AutoDisposeParameters = false)]
        public void SendPositionInformation(List<PositionInfo> listPosition)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "SendPositionInformation");
            _siatOperations.AnalyzePositionsDigest(listPosition);
        }

        [OperationBehavior]
        public List<Occurrence> ListAllOccurrences()
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "ListAllOccurrences");
            return _siatOperations.ListAllOccurrences();
        }

        public List<Occurrence> ListAllOccurrencesInWay(long wayId)
        {
            Debug.WriteLine("{0} - {1} Id:", DateTime.Now.ToLongTimeString(), "ListAllOccurrencesInWay", wayId);
            return _siatOperations.ListOccurrencesInWay(wayId);
        }

        [OperationBehavior]
        public void Alert(Alert alert)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "Alert");
            //_siatOperations.SendAlert(alert);
        }

        [OperationBehavior]
        public Way CurrentWay(double lat, double lon)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "CurrentWay");
            return _siatOperations.GetCurrentWay(lat,lon);
        }

        #endregion


    }
}
