using System;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Controllers.V1
{
    /// <summary>
    /// Business Logic to get and save the complete entity object 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBLLController<T1>
        where T1 : AGSCommon.Models.DataModels.AGSIdentity.AGSIdentityDataModel
    {
        string SaveOrUpdateModel(T1 model);

        void DeleteModel(string id);

        T1 GetModel(string id);
    }
}
