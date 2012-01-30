using SIAT.DAL.DataMappers;
using SIAT.DAL.EFModel;
using SIAT.DAL.IDataMappers;
using SIAT.DAL.Shared;

namespace SIAT.DAL
{
    public class SIATDataAccessLayer : IUnitOfWork
    {
        private bool isSaved;
        private SIATEntities _entities;
        private IOccurrencesDataMapper _occurrences;

        public SIATDataAccessLayer() : base()
        {
            isSaved = false;
            _entities = new SIATEntities();
        }

        public IOccurrencesDataMapper Occurrences
        {
            get { return _occurrences ?? (_occurrences = new OccurrencesDataMapper(_entities)); }
        }
        
        #region Implementation of IDisposable

        public void Dispose()
        {
            if (!isSaved)
            {
                Commit();
            }
            _entities.Dispose();
        }

        #endregion

        #region Implementation of IUnitOfWork

        public void Commit()
        {
            isSaved = true;
            _entities.SaveChanges();
        }

        public void Rollback()
        {
            
        }

        #endregion
    }
}
