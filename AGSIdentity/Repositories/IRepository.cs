using System;
namespace AGSIdentity.Repositories
{
    /// <summary>
    /// This interface serves for unit of work
    /// </summary>
    public interface IRepository : IDisposable
    {
        int Save();
    }
}
