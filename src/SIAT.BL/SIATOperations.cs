using System;
using System.Collections.Generic;
using System.Messaging;
using SIAT.BL.BusMessages;
using SIAT.BL.DTO;
using SIAT.DAL;
using System.Linq;
using SIAT.DAL.DataMappers;
using System.Threading.Tasks;

namespace SIAT.BL
{

    public class SIATOperations : ISIATOperations
    {


        #region Implementation of ISIATOperations

        public List<Occurrence> ListAllOccurrences()
        {
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                return siatDataAccessLayer.Occurrences.GetAll().Select(o =>
                        new Occurrence(new GeoPoint(o.lat, o.lon), o.intensity, siatDataAccessLayer.Ways.Get(o.wayId).@ref)).ToList();
            }
        }

        public List<Occurrence> ListOccurrences(GeoPoint geoPoint, double radious)
        {
            throw new NotImplementedException();
        }

        public List<Occurrence> ListOccurrencesInWay(long roadId)
        {
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                return siatDataAccessLayer.Occurrences.GetAllFromWay(roadId).Select(o =>
                    new Occurrence(new GeoPoint(o.lat, o.lon), o.intensity, siatDataAccessLayer.Ways.Get(o.wayId).@ref)).ToList();
            }
        }

        public Way GetCurrentWay(GeoPoint geoPoint)
        {
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                var way = siatDataAccessLayer.Ways.GetClosestWay(geoPoint.Latitude, geoPoint.Longitude);

                if (way == null)
                {
                    return null;
                }

                return new Way(way.id, way.@ref, way.name, way.type);
            }
        }

        public List<Occurrence> ListOccurrences(GeoPoint wayClosestToPoint)
        {
            throw new NotImplementedException();
        }

        public void SendAlert(Alert alert)
        {
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {

                //// todo add logic - ver se já existe alguma ocurrencia neste ponto
                //// add a stored procedure

                var node = siatDataAccessLayer.Nodes.GetClosestNode(alert.GeoPoint.Latitude, alert.GeoPoint.Longitude);
                if (node == null)
                {
                    return;
                }

                byte intensity = 30;

                switch (alert.Level)
                {
                    case AlertLevel.RoadBlock:
                        intensity = 0;
                        break;
                    case AlertLevel.MediumTraffic:
                        intensity = 10; // +/- 40km/h
                        break;
                    case AlertLevel.HighTraffic:
                        intensity = 5; // +/- 20km/h
                        break;
                }

                siatDataAccessLayer.Occurrences.Add(new DAL.EFModel.Occurrence()
                                                                   {
                                                                       lat = alert.GeoPoint.Latitude,
                                                                       lon = alert.GeoPoint.Longitude,
                                                                       intensity = intensity,
                                                                       precision = 100,
                                                                       description = null,
                                                                       idCurrentNode = node.id,
                                                                       wayId = node.wayId
                                                                   });
            }


        }

        private const string AnalyzePositionQueuePath = @".\private$\AnalyzePositionsWorker";

        public void AnalyzePositionsDigestAsync(List<PositionInfo> listPositionInfo, Action @delegate)
        {
            try
            {
                MessageQueue messageQueue = new MessageQueue(AnalyzePositionQueuePath);
                messageQueue.Send(listPositionInfo);
            }
            catch (MessageQueueException e)
            {
                if (!MessageQueue.Exists(AnalyzePositionQueuePath))
                {
                    MessageQueue messageQueue = MessageQueue.Create(AnalyzePositionQueuePath);
                    messageQueue.Send(listPositionInfo);
                }
            }
        }

        private const string WP7NotificationQueue = @".\private$\OccurrencesUpdate\WP7";

        //
        // this function must be run by multiple processes reading from a QUEUE
        //
        public void AnalyzePositionsDigest(List<PositionInfo> listPositionInfo)
        {
            using (SIATDataAccessLayer siatDataAccessLayer = new SIATDataAccessLayer())
            {
                foreach (var positionInfo in listPositionInfo)
                {
                    DAL.EFModel.Occurrence ocurrence = siatDataAccessLayer.Occurrences.GetClosest(positionInfo.GeoPoint.Latitude, positionInfo.GeoPoint.Longitude);

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
                                ocurrence.precision +=
                                    Convert.ToByte(CalcPrecision(Math.Abs(ocurrence.intensity - positionInfo.Speed)));
                            }
                            catch (OverflowException)
                            {
                                ocurrence.precision = (((int)ocurrence.precision) + CalcPrecision(Math.Abs(ocurrence.intensity - positionInfo.Speed))) > byte.MaxValue ? byte.MaxValue : byte.MinValue;
                            }
                        }

                        // update intesity
                        ocurrence.intensity = (byte)(((ocurrence.intensity * 4) + Convert.ToByte(positionInfo.Speed)) / 5);

                        //
                        // if precision is less than 45 or the intensity is more than 20m/s, remove the occurrence
                        //

                        if (ocurrence.precision < 45 || ocurrence.intensity > 20) // 20m/s = 70km/h todo alterar para que a intensidade tenha algo a ver com a estrada actual
                        {
                            siatDataAccessLayer.Occurrences.Delete(ocurrence.id);
                        }
                        else
                        {
                            siatDataAccessLayer.Occurrences.Update(ocurrence);
                        }
                    }
                    else
                    {
                        if (positionInfo.Speed < 5) // 10m/s = 40km/h
                        {
                            var node = siatDataAccessLayer.Nodes.GetClosestNode(positionInfo.GeoPoint.Latitude,
                                                                                            positionInfo.GeoPoint.Longitude);
                            ocurrence = new DAL.EFModel.Occurrence();
                            ocurrence.lat = positionInfo.GeoPoint.Latitude;
                            ocurrence.lon = positionInfo.GeoPoint.Longitude;
                            ocurrence.idCurrentNode = node.id;
                            ocurrence.wayId = node.wayId;
                            ocurrence.precision = 80; // precision start value
                            ocurrence.description = null;
                            ocurrence.intensity = (byte)positionInfo.Speed;

                            siatDataAccessLayer.Occurrences.Add(ocurrence);

                            //
                            // Send Information to Notification Systems
                            //
                            MessageQueue wp7MessageQueue = new MessageQueue(WP7NotificationQueue);
                            try
                            {
                                wp7MessageQueue.Send(new NewOccurrenceMessage()
                                                     {
                                                         WayId = ocurrence.wayId
                                                     });
                            }
                            catch (MessageQueueException e)
                            {
                                if (!MessageQueue.Exists(WP7NotificationQueue))
                                {
                                    MessageQueue messageQueue = MessageQueue.Create(WP7NotificationQueue);
                                    messageQueue.Send(new NewOccurrenceMessage()
                                    {
                                        WayId = ocurrence.wayId
                                    });
                                }
                            }
                        }
                    }

                }
            }
        }

        

        #endregion
    }

}
