using System.Collections.Generic;
using AspNetIdentity.Domain.CommonEntity;
using AspNetIdentity.Mapper.Common;
using AspNetIdentity.Model.CommonDto;
using AspNetIdentity.Model.IdentityDto.Role;
using AspNetIdentity.WebApi.Utility.RequestMessageProvider;

namespace AspNetIdentity.Mapper.Identity.Role
{
    public class RoleFactory : ModelFactory, IRoleFactory
    {
        public RoleFactory(IRequestMessageProvider requestMessageProvider) : base(requestMessageProvider)
        {
        }

        public TDto GetModel<TDto>(IEntityBase domainEntity) where TDto : DtoBase
        {
            if (TypesEqual<TDto, RoleDto>())
            {
                var roleEntity = (Domain.IdentityEntity.Role)domainEntity;

                return new RoleDto
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "My account",
                            Href = Url.Link("MyAccount", null),
                            Method = "GET"
                        }
                    }
                } as TDto;
            }
            if (TypesEqual<TDto, RoleForAdminDto>())
            {
                var roleEntity = (Domain.IdentityEntity.Role)domainEntity;

                return new RoleForAdminDto
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "Self",
                            Href = Url.Link("GetRole", new { id = roleEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Self by name",
                            Href = Url.Link("GetRoleByName", new { name = roleEntity.Name }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Role users",
                            Href = Url.Link("GetRoleUsers", new { id = roleEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "Change role name",
                            Href = Url.Link("ChangeRoleName", new { id = roleEntity.Id }),
                            Method = "PUT"
                        },
                        new Link
                        {
                            Rel = "Delete role",
                            Href = Url.Link("DeleteRole", new { id = roleEntity.Id }),
                            Method = "DELETE"
                        }
                    }
                } as TDto;
            }

            return null;
        }
    }
}