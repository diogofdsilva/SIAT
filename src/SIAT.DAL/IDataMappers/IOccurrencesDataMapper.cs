using System.Collections.Generic;
using SIAT.DAL.Shared;
using SIAT.Service.Contract.DTO;

namespace SIAT.DAL.IDataMappers
{
    public interface IOccurrencesDataMapper : IDalMapper<Occurrence, List<Occurrence>, int>
    {
        Occurrence GetClosest(double latitude, double longitude);
        List<Occurrence> GetAllFromWay(long id);
    }
}