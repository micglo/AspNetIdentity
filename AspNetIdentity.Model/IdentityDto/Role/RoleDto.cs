using System.Collections.Generic;
using AspNetIdentity.Model.CommonDto;

namespace AspNetIdentity.Model.IdentityDto.Role
{
    public class RoleDto : CommonDto<string>
    {
        public string Name { get; set; }
        public ICollection<Link> Links { get; set; }
    }
}