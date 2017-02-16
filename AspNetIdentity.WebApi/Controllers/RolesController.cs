using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.Mapper.Identity.Role;
using AspNetIdentity.Mapper.Identity.User;
using AspNetIdentity.Model.BindingModel.Role;
using AspNetIdentity.Model.IdentityDto.Role;
using AspNetIdentity.Model.IdentityDto.User;
using AspNetIdentity.WebApi.Utility.Filter;
using AspNetIdentity.WebApi.IdentityConfig;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Controllers
{
    [MyAuthorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        public RolesController(UserManager userManager, RoleManager roleManager, IUserFactory userFactory, IRoleFactory roleFactory)
            : base(userManager, roleManager, userFactory, roleFactory)
        {
        }


        [Route("", Name = "GetRoles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRoles()
        {
            var roles = await RoleManager.Roles.ToListAsync();
            var rolesDto = roles.Select(RoleFactory.GetModel<RoleForAdminDto>).ToList();
            return Ok(rolesDto);
        }


        [Route("{id:guid}", Name = "GetRole")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            var roleDto = RoleFactory.GetModel<RoleForAdminDto>(role);
            return Ok(roleDto);
        }


        [Route("{name}", Name = "GetRoleByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRoleByName(string name)
        {
            var role = await RoleManager.FindByNameAsync(name);

            if (role == null)
                return NotFound();

            var roleDto = RoleFactory.GetModel<RoleForAdminDto>(role);
            return Ok(roleDto);
        }


        [Route("{id:guid}/Users", Name = "GetRoleUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRoleUsers(string id)
        {
            var roleUsersList = new List<User>();
            var role = await RoleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            var roleUsers = role.Users;

            foreach (var roleUser in roleUsers)
            {
                var user = await UserManager.FindByIdAsync(roleUser.UserId);
                roleUsersList.Add(user);
            }

            var roleUsersDto = roleUsersList.Select(UserFactory.GetModel<UserForAdminDto>);
            return Ok(roleUsersDto);
        }

        [Route("", Name = "CreateRole")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRole([FromBody] RoleBindingModel roleModel)
        {
            if (roleModel == null)
                return BadRequest("Payload is empty!");

            var role = new Role { Name = roleModel.Name };
            IdentityResult createRoleResult = await RoleManager.CreateAsync(role);

            if (!createRoleResult.Succeeded)
                return GetErrorResult(createRoleResult);

            var roleDto = RoleFactory.GetModel<RoleForAdminDto>(role);
            Uri locationHeader = new Uri(Url.Link("GetRole", new { id = role.Id }));
            return Created(locationHeader, roleDto);
        }


        [Route("{id:guid}/ChangeName", Name = "ChangeRoleName")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangeRoleName([FromUri] string id, [FromBody] RoleBindingModel roleModel)
        {
            if (roleModel == null)
                return BadRequest("Payload is empty!");

            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            role.Name = roleModel.Name;
            IdentityResult createRoleResult = await RoleManager.UpdateAsync(role);

            if (!createRoleResult.Succeeded)
                return GetErrorResult(createRoleResult);

            var roleDto = RoleFactory.GetModel<RoleForAdminDto>(role);
            Uri locationHeader = new Uri(Url.Link("GetRole", new { id = role.Id }));
            return Created(locationHeader, roleDto);
        }


        [Route("{id:guid}/Delete", Name = "DeleteRole")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            IdentityResult result = await RoleManager.DeleteAsync(role);

            return result.Succeeded ? Ok() : GetErrorResult(result);
        }
    }
}