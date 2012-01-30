using System;

namespace SIAT.DAL.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}