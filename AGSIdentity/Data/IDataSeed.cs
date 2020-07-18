using System;
namespace AGSIdentity.Data
{
    public interface IDataSeed
    {
        void InitializeAuthenticationServer();

        void InitializeApplicationData();

        void RemoveAuthenticationServerData();

        void RemoveApplicationData();
    }
}
