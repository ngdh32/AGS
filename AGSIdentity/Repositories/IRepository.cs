using System;
namespace AGSIdentity.Repositories
{
    /// <summary>
    /// This interface serves for unit of work
    /// </summary>
    public interface IRepository : IDisposable
    {
        int Save();

        IUsersRepository UsersRepository { get; }

        IGroupsRepository GroupsRepository { get; }
        
        IFunctionClaimsRepository FunctionClaimsRepository { get; }
    }
}
