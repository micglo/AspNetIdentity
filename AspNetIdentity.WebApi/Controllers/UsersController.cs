using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.Mapper.Identity.Claim;
using AspNetIdentity.Mapper.Identity.Role;
using AspNetIdentity.Mapper.Identity.User;
using AspNetIdentity.Model.BindingModel.User;
using AspNetIdentity.Model.IdentityDto.Claim;
using AspNetIdentity.Model.IdentityDto.Role;
using AspNetIdentity.Model.IdentityDto.User;
using AspNetIdentity.WebApi.IdentityConfig;
using AspNetIdentity.WebApi.Utility.Filter;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Controllers
{
    [MyAuthorize(Roles = "Admin")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        public UsersController(UserManager userManager, RoleManager roleManager, IClaimFactory claimFactory, IUserFactory userFactory, IRoleFactory roleFactory)
            : base(userManager, roleManager, userFactory, roleFactory)
        {
            ClaimFactory = claimFactory;
        }


        [Route("", Name = "GetUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await UserManager.Users.ToListAsync();
            var usersDto = users.Select(UserFactory.GetModel<UserForAdminDto>).ToList();
            return Ok(usersDto);
        }


        [Route("{id:guid}", Name = "GetUser")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [Route("{userName}", Name = "GetUserByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserByName(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound();

            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [Route("{id:guid}/Claims", Name = "GetUserClaims")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserClaims(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var userClaims = user.Claims.ToList();

            var claimsDto = userClaims.Select(ClaimFactory.GetModel<ClaimDto>);
            return Ok(claimsDto);
        }


        [Route("{id:guid}/Roles", Name = "GetUserRoles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserRoles(string id)
        {
            var userRolesList = new List<Role>();
            var user = await UserManager.FindByIdAsync(id);
            var userRoles = user.Roles;

            foreach (var userRole in userRoles)
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                userRolesList.Add(role);
            }

            var userRolesDto = userRolesList.Select(RoleFactory.GetModel<RoleForAdminDto>);
            return Ok(userRolesDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("", Name = "CreateUser")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(CreateUserDto userModel)
        {
            if (userModel == null)
                return BadRequest("Payload is empty!");

            var userEntity = UserFactory.GetModel(userModel);
            var user = (User) userEntity;
            IdentityResult registerResult = await UserManager.CreateAsync(user, userModel.Password);

            if (!registerResult.Succeeded)
                return GetErrorResult(registerResult);

            var userDto = UserFactory.GetModel<UserForAdminDto>(userEntity);
            Uri locationHeader = new Uri(Url.Link("GetUser", new { id = user.Id }));
            return Created(locationHeader, userDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("{id:guid}/AddClaims", Name = "AddClaimsToUser")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaimsToUser(string id, [FromBody] UserClaimsBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var currentUserClaims = await UserManager.GetClaimsAsync(id);

            foreach (var claim in model.Claims)
            {
                if (!currentUserClaims.Any(c => c.Type.Equals(claim.Type)))
                    await UserManager.AddClaimAsync(user.Id, new Claim(claim.Type, claim.Value, ClaimValueTypes.String));
            }

            var claimsDto = user.Claims.Select(ClaimFactory.GetModel<ClaimDto>);
            return Ok(claimsDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("{id:guid}/AddRoles", Name = "AddRolesToUser")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRolesToUser(string id, [FromBody] UserRolesBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var rolesNotExists = model.Roles.Except(RoleManager.Roles.Select(x => x.Name)).ToArray();
            if (rolesNotExists.Any())
            {
                ModelState.AddModelError("", $"Roles '{string.Join(",", rolesNotExists)}' does not exixts.");
                return BadRequest(ModelState);
            }

            var currentUserRoles = await UserManager.GetRolesAsync(user.Id);
            var rolesToAdd = model.Roles.Except(currentUserRoles).ToArray();
            await UserManager.AddToRolesAsync(user.Id, rolesToAdd);

            var userRoles = user.Roles;
            var userRolesList = new List<Role>();

            foreach (var userRole in userRoles)
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                userRolesList.Add(role);
            }

            var userRolesDto = userRolesList.Select(RoleFactory.GetModel<RoleForAdminDto>);
            return Ok(userRolesDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("{id:guid}/RemoveClaims", Name = "RemoveClaimsFromUser")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveClaimsFromUser(string id, [FromBody] UserClaimsBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            foreach (var claim in model.Claims)
            {
                if (user.Claims.Any(c => c.ClaimType == claim.Type))
                    await UserManager.RemoveClaimAsync(user.Id,
                        new Claim(claim.Type, claim.Value, ClaimValueTypes.String));
            }

            var claimsDto = user.Claims.Select(ClaimFactory.GetModel<ClaimDto>);
            return Ok(claimsDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("{id:guid}/RemoveRoles", Name = "RemoveRolesFromUser")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveRolesFromUser(string id, [FromBody] UserRolesBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var rolesNotExists = model.Roles.Except(RoleManager.Roles.Select(x => x.Name)).ToArray();
            if (rolesNotExists.Any())
            {
                ModelState.AddModelError("", $"Roles '{string.Join(",", rolesNotExists)}' does not exixts.");
                return BadRequest(ModelState);
            }

            var currentUserRoles = await UserManager.GetRolesAsync(user.Id);
            var rolesToRemove = currentUserRoles.Intersect(model.Roles).ToArray();

            await UserManager.RemoveFromRolesAsync(user.Id, rolesToRemove);

            var userRoles = user.Roles;
            var userRolesList = new List<Role>();

            foreach (var userRole in userRoles)
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                userRolesList.Add(role);
            }

            var userRolesDto = userRolesList.Select(RoleFactory.GetModel<RoleForAdminDto>);
            return Ok(userRolesDto);
        }


        [Route("{id:guid}/Activate", Name = "ActivateUser")]
        [HttpGet]
        public async Task<IHttpActionResult> ActivateUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsActive = true;

            await UserManager.UpdateAsync(user);
            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [Route("{id:guid}/Unban", Name = "UnbanUser")]
        [HttpGet]
        public async Task<IHttpActionResult> UnbanUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsBanned = false;

            await UserManager.UpdateAsync(user);
            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [Route("{id:guid}/Deactivate", Name = "DeactivateUser")]
        [HttpGet]
        public async Task<IHttpActionResult> DeactivateUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsActive = false;

            await UserManager.UpdateAsync(user);
            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [Route("{id:guid}/Ban", Name = "BanUser")]
        [HttpGet]
        public async Task<IHttpActionResult> BanUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsBanned = true;

            await UserManager.UpdateAsync(user);
            var userDto = UserFactory.GetModel<UserForAdminDto>(user);
            return Ok(userDto);
        }


        [MyAuthorize(Roles = "SuperAdmin")]
        [Route("{id:guid}/Delete", Name = "DeleteUser")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            IdentityResult result = await UserManager.DeleteAsync(user);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }

        //todo list user claims add claims to user remove claims from user
    }
}
