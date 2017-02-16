using System;
using AspNetIdentity.Domain.CommonEntity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.Domain.IdentityEntity
{
    public class Role : IdentityRole<string, UserRole>, IEntityBase
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}