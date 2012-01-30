using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SIAT.DAL;
using SIAT.Service.Contract;
using SIAT.Service.Contract.DTO;

namespace SIAT.IISServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single,IncludeExceptionDetailInFaults = true)]
    public class SIATService : ISIATService
    {
        [OperationBehavior]
        public void InsertNewOccurrence(Occurrence occurrence)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "InsertNewOccurrence");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                siatDataAccessLayer.Occurrences.Add(occurrence);
            }
        }

        public Occurrence GetClosestOccurrence(double latitude, double longitude)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "GetClosestOccurrence");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                return siatDataAccessLayer.Occurrences.GetClosest(latitude, longitude);
            }
        }

        public Occurrence GetOccurrence(int occurrenceId)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "GetOccurrence");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                return siatDataAccessLayer.Occurrences.Get(occurrenceId);
            }
        }

        public void UpdateOccurrence(Occurrence occurrence)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "UpdateOccurrence");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                siatDataAccessLayer.Occurrences.Update(occurrence);
            }
        }

        public void RemoveOccurrence(int occurrenceId)
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "RemoveOccurrence");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                siatDataAccessLayer.Occurrences.Delete(occurrenceId);
            }
        }

        public List<Occurrence> ListAllOccurrences()
        {
            Debug.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "ListAllOccurrences");
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                return siatDataAccessLayer.Occurrences.GetAll().ToList();
            }
        }

        public List<Occurrence> ListAllOccurrencesInWay(long wayId)
        {
            Debug.WriteLine("{0} - {1} Id:{2}", DateTime.Now.ToLongTimeString(), "ListAllOccurrencesInWay", wayId);
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {

                return siatDataAccessLayer.Occurrences.GetAllFromWay(wayId).ToList();
            }
        }
        
    }
}
