using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetIdentity.Model.BindingModel.Helper;

namespace AspNetIdentity.Model.BindingModel.User
{
    public class UserClaimsBindingModel
    {
        [Required]
        public List<ClaimModel> Claims { get; set; }
    }
}