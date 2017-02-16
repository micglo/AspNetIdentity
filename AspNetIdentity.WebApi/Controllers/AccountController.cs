using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.Mapper.Identity.Role;
using AspNetIdentity.Mapper.Identity.User;
using AspNetIdentity.Model.BindingModel.Account;
using AspNetIdentity.Model.IdentityDto.Role;
using AspNetIdentity.Model.IdentityDto.User;
using AspNetIdentity.WebApi.IdentityConfig;
using AspNetIdentity.WebApi.Utility.Filter;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Controllers
{
    [MyAuthorize]
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        public AccountController(UserManager userManager, RoleManager roleManager, IUserFactory userFactory, IRoleFactory roleFactory)
            : base(userManager, roleManager, userFactory, roleFactory)
        {
        }


        [AllowAnonymous]
        [Route("Register", Name = "Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(AccountRegisterBindingModel userModel)
        {
            if (userModel == null)
                return BadRequest("Payload is empty!");

            var userEntity = UserFactory.GetModel(userModel);
            var user = (User)userEntity;
            IdentityResult registerResult = await UserManager.CreateAsync(user, userModel.Password);

            if (!registerResult.Succeeded)
                return GetErrorResult(registerResult);

            await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account.");
            var userDto = UserFactory.GetModel<UserDto>(userEntity);
            Uri locationHeader = new Uri(Url.Link("GetUser", new { id = user.Id }));
            return Created(locationHeader, userDto);
        }


        [AllowAnonymous]
        [Route("ResendActivationLink", Name = "ResendActivationLink")]
        [HttpPost]
        public async Task<IHttpActionResult> ResendActivationLink(ResendActivationLinkBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound();

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is already confirmed.");
                return BadRequest(ModelState);
            }

            var result = await UserManager.CheckPasswordAsync(user, model.Password);

            if (result)
            {
                await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account.");
                return Ok();
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return BadRequest(ModelState);
        }


        [Route("MyAccount", Name = "MyAccount")]
        [HttpGet]
        public async Task<IHttpActionResult> MyAccount()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            var userDto = UserFactory.GetModel<UserDto>(user);
            return Ok(userDto);
        }

        [Route("MyRoles", Name = "MyRoles")]
        [HttpGet]
        public async Task<IHttpActionResult> MyRoles()
        {
            var myRolesList = new List<Role>();
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var myRoles = user.Roles;

            foreach (var myRole in myRoles)
            {
                var role = await RoleManager.FindByIdAsync(myRole.RoleId);
                myRolesList.Add(role);
            }

            var myRolesDto = myRolesList.Select(RoleFactory.GetModel<RoleDto>);
            return Ok(myRolesDto);
        }

        [Route("ChangePassword", Name = "ChangePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (model == null)
                return BadRequest("Payload is empty!");

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            return result.Succeeded ? Ok() : GetErrorResult(result);
        }


        [Route("Delete", Name = "DeleteAccount")]
        [HttpGet]
        public async Task<IHttpActionResult> DeleteAccount()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
                return NotFound();

            user.IsActive = false;

            await UserManager.UpdateAsync(user);

            return Ok();
        }


        [AllowAnonymous]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError("", "User Id and Code are required.");
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", $"Email: {user.Email} is already confirmed.");
                return BadRequest(ModelState);
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return result.Succeeded ? Ok() : GetErrorResult(result);
        }


        #region Helpers

        private async Task SendEmailConfirmationTokenAsync(string userId, string subject)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { userId, code }));
            await UserManager.SendEmailAsync(userId, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        #endregion
    }
}