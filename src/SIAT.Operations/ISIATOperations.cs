using System;
using System.Collections.Generic;
using SIAT.Service.Contract.DTO;
using SIAT.UserInfo.Contract.DTO;
using MS.OSM.Querys.DTO;

namespace SIAT.Operations
{
    public interface ISIATOperations
    {
        List<Occurrence> ListAllOccurrences();
        List<Occurrence> ListOccurrencesInWay(long wayId);
        Occurrence FindOccurrence(int id);

        void AnalyzePositionsDigest(List<PositionInfo> listPositionInfo);
        void AnalyzePositionsDigestAsync(List<PositionInfo> listPositionInfo);
        void AnalyzePositionsDigestQueueAsync(List<PositionInfo> listPositionInfo);
        void SendAlert(Occurrence alert);

        Way GetCurrentWay(double lat, double lon);
        
        User Login(string email, string pass);
        User GetUserByFacebookId(long facebookId);
        User GetUserByEmail(string email);
        User CreateNewUser(User user);

        ServiceProxysManager ProxysManager { get; }
    }

    public class PositionInfo
    {
        public double Lat { get; set; }

        public double Lon { get; set; }

        public double Speed { get; set; }

        public DateTime Date { get; set; }
    }
}