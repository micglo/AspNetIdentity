using System.Collections.Generic;
using AspNetIdentity.Model.CommonDto;

namespace AspNetIdentity.Model.IdentityDto.Claim
{
    public class ClaimDto : CommonDto<int>
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public List<Link> Links { get; set; }
    }
}