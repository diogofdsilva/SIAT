using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;
using SIAT.DAL.EFModel;
using SIAT.DAL.IDataMappers;
using SIAT.Service.Contract.DTO;

namespace SIAT.DAL.DataMappers
{
    public class OccurrencesDataMapper : IOccurrencesDataMapper
    {
        private SIATEntities _entities;

        public OccurrencesDataMapper(SIATEntities entities)
        {
            _entities = entities;
        }

        #region Implementation of IDalMapper<Occurrence,List<Occurrence>,int>

        private const string InsertOccurrenceQuery =
            "INSERT INTO Occurrence (lat,lon,geo,wayId,idCurrentNode, idLastNode,intensity,precision,description) VALUES (@lat,@lon,@p,@wayId,@idCurrentNode,@idLastNode,@intensity,@prec,@description);";

        public Occurrence Add(Occurrence e)
        {
            SqlGeographyBuilder gLocationBuilder = new SqlGeographyBuilder();
            gLocationBuilder.SetSrid(4326);
            gLocationBuilder.BeginGeography(OpenGisGeographyType.Point);
            gLocationBuilder.BeginFigure(e.Latitude, e.Longitude);
            gLocationBuilder.EndFigure();
            gLocationBuilder.EndGeography();

            // Create the geometry parameter  
            SqlParameter parameter =
                new SqlParameter("@p", gLocationBuilder.ConstructedGeography);
            parameter.UdtTypeName = "geography";

            // Description Nullable parameter
            SqlParameter descriptionParameter = new SqlParameter("@description", SqlDbType.Text);
            descriptionParameter.IsNullable = true; 
            descriptionParameter.Value = (object) e.Description ?? DBNull.Value;

            // idCurrentNode Nullable parameter
            SqlParameter idCurrentNodeParam = new SqlParameter("@idCurrentNode", SqlDbType.BigInt);
            idCurrentNodeParam.IsNullable = true;
            idCurrentNodeParam.Value = (object)e.IdCurrentNode ?? DBNull.Value;

            // idLastNode Nullable parameter
            SqlParameter idLastNodeParam = new SqlParameter("@idLastNode", SqlDbType.BigInt);
            idLastNodeParam.IsNullable = true;
            idLastNodeParam.Value = (object)e.IdLastNode ?? DBNull.Value;

            _entities.ExecuteStoreCommand(InsertOccurrenceQuery, new SqlParameter("@lat", e.Latitude),
                new SqlParameter("@lon", e.Longitude), parameter, new SqlParameter("@wayId", e.WayId), 
                idCurrentNodeParam, idLastNodeParam, new SqlParameter("@intensity", e.Intensity),
                new SqlParameter("@prec", e.Precision), descriptionParameter);
            

            return e;
        }

        public Occurrence Get(int key)
        {
            return _entities.Occurrences.SingleOrDefault(o=>o.Id==key); 
        }

        public List<Occurrence> GetAll()
        {
            return _entities.Occurrences.ToList();
        }

        public Occurrence Update(Occurrence e)
        {
            var ret = _entities.Occurrences.ApplyCurrentValues(e);
            
            return ret;
        }

        public Occurrence Delete(int key)
        {
            Occurrence occurrence = _entities.Occurrences.SingleOrDefault(o => o.Id == key);

            _entities.Occurrences.DeleteObject(occurrence);

            return occurrence;
        }

        #endregion

        public Occurrence GetClosest(double latitude, double longitude)
        {
            return _entities.GetClosestOccurrence(latitude, longitude).FirstOrDefault();
        }

        public List<Occurrence> GetAllFromWay(long id)
        {
            return _entities.Occurrences.Where(pred => pred.WayId == id).ToList();
        }

    }
}
