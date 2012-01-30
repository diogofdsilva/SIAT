using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using SIAT.Service.Contract.DTO;

namespace SIAT.DAL.EFModel
{
    public class SIATEntities : ObjectContext
    {
        private const string ContainerName = "Entities";
       
        public SIATEntities()
            : base("name=SIATEntities", ContainerName)  
        {
            
        }

        public SIATEntities(string connectionString)
            : base(connectionString, ContainerName)
        {
            
        }

        public ObjectSet<Occurrence> Occurrences
        {
            get
            {
                return _occurrences ?? (_occurrences = CreateObjectSet<Occurrence>());
            }
        }
        private ObjectSet<Occurrence> _occurrences;

        public ObjectResult<Occurrence> GetClosestOccurrence(double latitude, double longitude)
        {
            return base.ExecuteFunction<Occurrence>("GetClosestOccurrence",
                                                  new ObjectParameter("lat", latitude),
                                                  new ObjectParameter("lon", longitude));
        }
    }
}
