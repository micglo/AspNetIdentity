using AspNetIdentity.Domain.CommonEntity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.Domain.IdentityEntity
{
    public class UserLogin : IdentityUserLogin<string>, IEntityBase
    {
        
    }
}