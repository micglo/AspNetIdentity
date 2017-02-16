using AspNetIdentity.Model.CommonDto;
using System;
using System.Collections.Generic;

namespace AspNetIdentity.Model.IdentityDto.User
{
    public class UserDto : CommonDto<string>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinDate { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public ICollection<Link> Links { get; set; }
    }
}