using System;
using System.Collections.Generic;
using System.Messaging;
using System.Threading.Tasks;
using SIAT.Operations.Messages;
using MS.OSM.Querys.DTO;
using SIAT.Service.Contract.DTO;
using SIAT.UserInfo.Contract.DTO;

namespace SIAT.Operations
{
    public class SIATOperations : ISIATOperations
    {
        private const string WP7NotificationQueue = @".\private$\OccurrencesUpdate_WP7";
        private const string AnalyzePositionQueuePath = @".\private$\AnalyzePositionsWorker";

        private ServiceProxysManager _proxysManager;

        public ServiceProxysManager ProxysManager { 
            get
            {
                return _proxysManager;
            }
        }


        public SIATOperations()
        {
            _proxysManager = new ServiceProxysManager();
        }

        #region ISIATOperations Members

        public Way GetCurrentWay(double lat, double lon)
        {
            return _proxysManager.OSMService.Service.GetClosestWay(lat, lon);
        }

        public List<Occurrence> ListAllOccurrences()
        {
            var list = _proxysManager.SIATService.Service.ListAllOccurrences();

            foreach (Occurrence occurrence in list)
            {
                var way = _proxysManager.OSMService.Service.GetWay(occurrence.WayId);

                if (way != null)
                {
                    if (way.Ref != null)
                    {
                        occurrence.WayName = way.Ref;
                    }
                    else if (way.Name != null)
                    {
                        occurrence.WayName = way.Name;
                    }
                    else
                    {
                        occurrence.WayName = "N/A";
                    }
                }
                else
                {
                    occurrence.WayName = "N/A";
                }
            }

            return list;
        }

        public List<Occurrence> ListOccurrencesInWay(long wayId)
        {

            return _proxysManager.SIATService.Service.ListAllOccurrencesInWay(wayId);

        }

        public void AnalyzePositionsDigestAsync(List<PositionInfo> listPositionInfo)
        {
            Task.Factory.StartNew(() => AnalyzePositionsDigest(listPositionInfo));
        }

        public void AnalyzePositionsDigestQueueAsync(List<PositionInfo> listPositionInfo)
        {
            try
            {
                MessageQueue messageQueue = new MessageQueue(AnalyzePositionQueuePath);
                messageQueue.Send(listPositionInfo);
            }
            catch (MessageQueueException)
            {
                if (!MessageQueue.Exists(AnalyzePositionQueuePath))
                {
                    MessageQueue messageQueue = MessageQueue.Create(AnalyzePositionQueuePath);
                    messageQueue.Send(listPositionInfo);
                }
            }
        }

        public void AnalyzePositionsDigest(List<PositionInfo> listPositionInfo)
        {

            foreach (PositionInfo positionInfo in listPositionInfo)
            {
                Occurrence ocurrence = _proxysManager.SIATService.Service.GetClosestOccurrence(positionInfo.Lat, positionInfo.Lon);

                if (ocurrence != null)
                {
                    //
                    // update precision
                    // check for possible byte overflow
                    //
                    checked
                    {
                        try
                        {
                            ocurrence.Precision +=
                                Convert.ToByte(CalcPrecision(Math.Abs(ocurrence.Intensity - positionInfo.Speed)));
                        }
                        catch (OverflowException)
                        {
                            ocurrence.Precision = ((ocurrence.Precision) +
                                                   CalcPrecision(Math.Abs(ocurrence.Intensity - positionInfo.Speed))) >
                                                  byte.MaxValue
                                                      ? byte.MaxValue
                                                      : byte.MinValue;
                        }
                    }

                    // update intesity
                    ocurrence.Intensity = (byte)(((ocurrence.Intensity * 4) + Convert.ToByte(positionInfo.Speed)) / 5);

                    //
                    // if precision is less than 45 or the intensity is more than 20m/s, remove the occurrence
                    //

                    if (ocurrence.Precision < 45 || ocurrence.Intensity > 20)
                    // 20m/s = 70km/h todo alterar para que a intensidade tenha algo a ver com a estrada actual
                    {
                        _proxysManager.SIATService.Service.RemoveOccurrence(ocurrence.Id);
                    }
                    else
                    {
                        _proxysManager.SIATService.Service.UpdateOccurrence(ocurrence);
                    }
                }
                else
                {
                    if (positionInfo.Speed < 10) // 10m/s = 40km/h
                    {
                        Way way = _proxysManager.OSMService.Service.GetClosestWay(positionInfo.Lat,
                                                             positionInfo.Lon);

                        Node node = _proxysManager.OSMService.Service.GetClosestWayNode(positionInfo.Lat, positionInfo.Lon, way.Id);

                        ocurrence = new Occurrence();
                        ocurrence.Latitude = positionInfo.Lat;
                        ocurrence.Longitude = positionInfo.Lon;
                        ocurrence.IdCurrentNode = node.Id;
                        ocurrence.WayId = way.Id;
                        ocurrence.Precision = 80; // precision start value
                        ocurrence.Description = null;
                        ocurrence.Intensity = (byte)positionInfo.Speed;

                        _proxysManager.SIATService.Service.InsertNewOccurrence(ocurrence);

                        //
                        // Send Information to Notification Systems
                        //

                        //MessageQueue wp7MessageQueue = new MessageQueue(WP7NotificationQueue);
                        //try
                        //{
                        //    wp7MessageQueue.Send(new NewOccurrenceMessage()
                        //    {
                        //        WayId = ocurrence.WayId
                        //    });
                        //}
                        //catch (MessageQueueException)
                        //{
                        //    if (!MessageQueue.Exists(WP7NotificationQueue))
                        //    {
                        //        MessageQueue messageQueue = MessageQueue.Create(WP7NotificationQueue);
                        //        messageQueue.Send(new NewOccurrenceMessage()
                        //        {
                        //            WayId = ocurrence.WayId
                        //        });
                        //    }
                        //}
                    }
                }
            }
            return;

        }

        public void SendAlert(Occurrence alert)
        {

            Occurrence ocurrence = new Occurrence();
            ocurrence.IdCurrentNode = _proxysManager.OSMService.Service.GetClosestNode(alert.Latitude, alert.Longitude).Id;
            ocurrence.Latitude = alert.Latitude;
            ocurrence.Longitude = alert.Longitude;
            ocurrence.Precision = 100;
            ocurrence.Description = alert.Description;
            ocurrence.Intensity = alert.Intensity;
            

            Way way = _proxysManager.OSMService.Service.GetClosestWay(alert.Latitude, alert.Longitude);
            ocurrence.WayId = way.Id;
            ocurrence.WayName = way.Name;

            _proxysManager.SIATService.Service.InsertNewOccurrence(ocurrence);

        }

        public User Login(string email, string pass)
        {
            return _proxysManager.UserInfoService.Service.Login(email, pass);

        }

        public User GetUserByFacebookId(long facebookId)
        {
            return _proxysManager.UserInfoService.Service.GetUserByFacebookId(facebookId);

        }

        public User GetUserByEmail(string email)
        {
            return _proxysManager.UserInfoService.Service.GetUserByEmail(email);

        }

        public User CreateNewUser(User user)
        {
            return _proxysManager.UserInfoService.Service.NewUser(user);
        }


        public Occurrence FindOccurrence(int id)
        {
            var occurrence = _proxysManager.SIATService.Service.GetOccurrence(id);
            occurrence.WayName = _proxysManager.OSMService.Service.GetWay(occurrence.WayId).Ref;

            return occurrence;
        }

        #endregion

        #region PrecisionLinearEquation

        private const double PrecisionM = (30 / -7);
        private const double PrecisionB = 30;

        private double CalcPrecision(double speedAvg)
        {
            return (speedAvg - PrecisionB) / PrecisionM;
        }

        #endregion PrecisionLinearEquation



    }
}