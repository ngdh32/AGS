using System;
namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    /// <summary>
    /// The reason why IRepository is dealing with AGSIdentityEntity instead of directly using Data Models in AGSCommon
    /// is because if Data Models in AGSCommon was used, then Repositories would be referenced among repositories
    /// which could be problematic as it is a better practice keeping repository having no linkage with other repositories
    /// in the repository class itself since repository should be as simple as possible
    /// and it could end up with circular reference. 
    /// For example, if AGSCommon.Models.DataModels.AGSIdentity.AGSGroup was used in IGroupRepository,
    /// When implementing Get(), it had to reference IFunctionClaimRepository in order to get the FunctionClaim object
    /// or otherwise it returned an incomplete object (null in FunctionClaim) which is apparently not ideal.
    /// With this class, it can ensured that Entity class is linked to other entity with primitive types instead of an object
    /// which is much closer to a database structure
    /// </summary>
    public class AGSIdentityEntity
    {
        public AGSIdentityEntity()
        {
        }
    }
}
