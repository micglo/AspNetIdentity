using AspNetIdentity.Model.BindingModel.Account;

namespace AspNetIdentity.Model.IdentityDto.User
{
    public class CreateUserDto : AccountRegisterBindingModel
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;
        public bool EmailConfirmed { get; set; } = true;
        public bool PhoneNumberConfirmed { get; set; } = true;
        public bool TwoFactorEnabled { get; set; } = false;
        public bool LockoutEnabled { get; set; } = false;
    }
}