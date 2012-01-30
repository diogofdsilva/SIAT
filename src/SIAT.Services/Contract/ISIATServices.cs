using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MS.OSM.Querys.DTO;
using SIAT.Operations;
using SIAT.Service.Contract.DTO;

namespace SIAT.Services.Contract
{
    [ServiceContract]
    public interface ISIATServices
    {
        [OperationContract]
        void SendPositionInformation(List<PositionInfo> listPosition);

        [OperationContract]
        List<Occurrence> ListAllOccurrences();

        [OperationContract]
        List<Occurrence> ListAllOccurrencesInWay(long wayId);

        [OperationContract]
        void Alert(Alert alert);

        [OperationContract]
        Way CurrentWay(double lat, double lon);
    }
}
