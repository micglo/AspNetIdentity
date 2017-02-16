using System.Web.Http;
using AspNetIdentity.Mapper.Identity.Claim;
using AspNetIdentity.Mapper.Identity.Role;
using AspNetIdentity.Mapper.Identity.User;
using AspNetIdentity.WebApi.IdentityConfig;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected UserManager UserManager;
        protected RoleManager RoleManager;

        protected IClaimFactory ClaimFactory;
        protected IUserFactory UserFactory;
        protected IRoleFactory RoleFactory;

        protected BaseApiController(UserManager userManager, RoleManager roleManager, IUserFactory userFactory, IRoleFactory roleFactory)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            UserFactory = userFactory;
            RoleFactory = roleFactory;
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}