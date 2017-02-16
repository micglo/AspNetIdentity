using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.Mapper.Identity.Claim;
using AspNetIdentity.Model.IdentityDto.Claim;
using AspNetIdentity.WebApi.IdentityConfig;
using AspNetIdentity.WebApi.Utility.Filter;

namespace AspNetIdentity.WebApi.Controllers
{
    [MyAuthorize]
    [RoutePrefix("api/claims")]
    public class ClaimsController : ApiController
    {
        private readonly UserManager _userManager;
        private readonly IClaimFactory _claimFactory;

        public ClaimsController(UserManager userManager, IClaimFactory claimFactory)
        {
            _userManager = userManager;
            _claimFactory = claimFactory;
        }


        [Route("")]
        public IHttpActionResult GetClaims()
        {
            //zczytuje dane z tokena
            var identity = User.Identity as ClaimsIdentity;

            var userClaims3 = identity?.Claims.Select(c => new
            {
                subject = c.Subject.Name,
                type = c.Type,
                value = c.Value
            }).ToList();

            return Ok(userClaims3);
        }


        [MyAuthorize(Roles = "Admin")]
        [Route("{userId:guid}/{id:int}", Name = "GetClaim")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClaim(string userId, int id)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var claim = user.Claims.SingleOrDefault(c => c.Id == id);
            var claimDto = _claimFactory.GetModel<ClaimDto>(claim);
            return Ok(claimDto);
        }
    }
}