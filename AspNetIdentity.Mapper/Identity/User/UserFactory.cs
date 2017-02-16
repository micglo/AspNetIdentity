using System;
using System.Collections.Generic;
using AspNetIdentity.Domain.CommonEntity;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.Mapper.Common;
using AspNetIdentity.Model.BindingModel.Account;
using AspNetIdentity.Model.CommonDto;
using AspNetIdentity.Model.IdentityDto.User;
using AspNetIdentity.WebApi.Utility.RequestMessageProvider;

namespace AspNetIdentity.Mapper.Identity.User
{
    public class UserFactory : ModelFactory, IUserFactory
    {
        public UserFactory(IRequestMessageProvider requestMessageProvider) : base(requestMessageProvider)
        {
        }

        public TDto GetModel<TDto>(IEntityBase domainEntity) where TDto : DtoBase
        {
            if (TypesEqual<TDto, UserDto>())
            {
                var userEntity = (Domain.IdentityEntity.User)domainEntity;

                return new UserDto
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    UserName = userEntity.UserName,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Gender = GetGenderString(userEntity.Gender),
                    BirthDate = userEntity.BirthDate,
                    JoinDate = userEntity.JoinDate,
                    Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "My account",
                            Href = Url.Link("MyAccount", null),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "My roles",
                            Href = Url.Link("MyRoles", null),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Resend activation link",
                            Href = Url.Link("ResendActivationLink", null),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Change password",
                            Href = Url.Link("ChangePassword", null),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Delete account",
                            Href = Url.Link("DeleteAccount", null),
                            Method = "GET"
                        }
                    }
                } as TDto;
            }
            if (TypesEqual<TDto, UserForAdminDto>())
            {
                var userEntity = (Domain.IdentityEntity.User)domainEntity;

                return new UserForAdminDto
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    UserName = userEntity.UserName,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Gender = GetGenderString(userEntity.Gender),
                    BirthDate = userEntity.BirthDate,
                    JoinDate = userEntity.JoinDate,
                    AccessFailedCount = userEntity.AccessFailedCount,
                    LockoutEndDateUtc = userEntity.LockoutEndDateUtc,
                    PhoneNumber = userEntity.PhoneNumber,
                    IsActive = userEntity.IsActive,
                    IsBanned = userEntity.IsBanned,
                    EmailConfirmed = userEntity.EmailConfirmed,
                    PhoneNumberConfirmed = userEntity.PhoneNumberConfirmed,
                    TwoFactorEnabled = userEntity.TwoFactorEnabled,
                    LockoutEnabled = userEntity.LockoutEnabled,
                    Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "Self",
                            Href = Url.Link("GetUser", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Self by name",
                            Href = Url.Link("GetUserByName", new { userName = userEntity.UserName }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "User claims",
                            Href = Url.Link("GetUserClaims", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "User roles",
                            Href = Url.Link("GetUserRoles", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Add claims to user",
                            Href = Url.Link("AddClaimsToUser", new { id = userEntity.Id }),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Add roles to user",
                            Href = Url.Link("AddRolesToUser", new { id = userEntity.Id }),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Remove claims from user",
                            Href = Url.Link("RemoveClaimsFromUser", new { id = userEntity.Id }),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Remove roles from user",
                            Href = Url.Link("RemoveRolesFromUser", new { id = userEntity.Id }),
                            Method = "POST"
                        },
                        new Link
                        {
                            Rel = "Activate user",
                            Href = Url.Link("ActivateUser", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Unban user",
                            Href = Url.Link("UnbanUser", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Deactivate",
                            Href = Url.Link("DeactivateUser", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Ban user",
                            Href = Url.Link("BanUser", new { id = userEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Delete user",
                            Href = Url.Link("DeleteUser", new { id = userEntity.Id }),
                            Method = "DELETE"
                        }
                    }
                } as TDto;
            }

            return null;
        }

        public  IEntityBase GetModel(DtoBase dtoModel)
        {
            if (TypesEqual<AccountRegisterBindingModel>(dtoModel))
            {
                var userDto = (AccountRegisterBindingModel)dtoModel;
                return new Domain.IdentityEntity.User
                {
                    Email = userDto.Email,
                    UserName = string.IsNullOrEmpty(userDto.UserName) ? userDto.Email : userDto.UserName,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Gender = GetGender(userDto.Gender),
                    BirthDate = userDto.BirthDate,
                    JoinDate = DateTime.Now,
                    IsBanned = false,
                    IsActive = true
                };
            }
            if (TypesEqual<CreateUserDto>(dtoModel))
            {
                var userDto = (CreateUserDto)dtoModel;
                return new Domain.IdentityEntity.User
                {
                    Email = userDto.Email,
                    UserName = string.IsNullOrEmpty(userDto.UserName) ? userDto.Email : userDto.UserName,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Gender = GetGender(userDto.Gender),
                    BirthDate = userDto.BirthDate,
                    JoinDate = DateTime.Now,
                    PhoneNumber = userDto.PhoneNumber,
                    IsBanned = userDto.IsBanned,
                    IsActive = userDto.IsActive,
                    EmailConfirmed = userDto.EmailConfirmed,
                    PhoneNumberConfirmed = userDto.PhoneNumberConfirmed,
                    TwoFactorEnabled = userDto.TwoFactorEnabled,
                    LockoutEnabled = userDto.LockoutEnabled
                };
            }

            return null;
        }

        private Gender GetGender(string gender)
        {
            switch (gender.ToUpper())
            {
                case "FEMALE":
                    return Gender.Female;
                default:
                    return Gender.Male;
            }
        }

        private string GetGenderString(Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "female";
                default:
                    return "male";
            }
        }
    }
}