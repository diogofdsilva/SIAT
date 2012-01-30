using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIAT.BL.DTO;

namespace SIAT.BL
{
    public interface ISIATOperations
    {
        List<Occurrence> ListAllOccurrences();

        List<Occurrence> ListOccurrences(GeoPoint geoPoint, double radious);

        List<Occurrence> ListOccurrences(GeoPoint wayClosestToPoint);

        List<Occurrence> ListOccurrencesInWay(long roadId);

        SIAT.OSM.DAL.EFModel.Way GetCurrentWay(GeoPoint geoPoint);

        void SendAlert(Alert alert);

        void AnalyzePositionsDigest(List<PositionInfo> listPositionInfo);
    }
}
 