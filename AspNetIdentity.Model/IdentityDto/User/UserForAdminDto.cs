using System;

namespace AspNetIdentity.Model.IdentityDto.User
{
    public class UserForAdminDto : UserDto
    {
        public string PhoneNumber { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}