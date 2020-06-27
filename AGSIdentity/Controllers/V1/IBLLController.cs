using System;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Controllers.V1
{
    /// <summary>
    /// Business Logic to get and save the complete entity object 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBLLController<T1>
        where T1 : AGSIdentityEntity
    {
        string SaveModel(T1 model);

        int UpdateModel(T1 model); 

        void DeleteModel(string id);

        T1 GetModel(string id);
    }
}
