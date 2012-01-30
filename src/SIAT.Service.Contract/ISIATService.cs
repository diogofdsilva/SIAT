using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SIAT.Service.Contract.DTO;

namespace SIAT.Service.Contract
{
    [ServiceContract]
    public interface ISIATService
    {
        [OperationContract]
        void InsertNewOccurrence(Occurrence occurrence);

        [OperationContract]
        Occurrence GetClosestOccurrence(double latitude, double longitude);

        [OperationContract]
        Occurrence GetOccurrence(int occurrenceId);

        [OperationContract]
        void UpdateOccurrence(Occurrence occurrence);

        [OperationContract]
        void RemoveOccurrence(int occurrenceId);

        [OperationContract]
        List<Occurrence> ListAllOccurrences();

        [OperationContract]
        List<Occurrence> ListAllOccurrencesInWay(long wayId);

    }
}
