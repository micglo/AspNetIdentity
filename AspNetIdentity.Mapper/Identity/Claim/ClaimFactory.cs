using System.Collections.Generic;
using AspNetIdentity.Domain.CommonEntity;
using AspNetIdentity.Mapper.Common;
using AspNetIdentity.Model.CommonDto;
using AspNetIdentity.Model.IdentityDto.Claim;
using AspNetIdentity.WebApi.Utility.RequestMessageProvider;

namespace AspNetIdentity.Mapper.Identity.Claim
{
    public class ClaimFactory : ModelFactory, IClaimFactory
    {
        public ClaimFactory(IRequestMessageProvider requestMessageProvider) : base(requestMessageProvider)
        {
        }

        public TDto GetModel<TDto>(IEntityBase domainEntity) where TDto : DtoBase
        {
            if (TypesEqual<TDto, ClaimDto>())
            {
                var claimEntity = (Domain.IdentityEntity.UserClaim)domainEntity;

                return new ClaimDto
                {
                    Id = claimEntity.Id,
                    UserId = claimEntity.UserId,
                    ClaimType = claimEntity.ClaimType,
                    ClaimValue = claimEntity.ClaimValue,
                    Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "Self",
                            Href = Url.Link("GetClaim",  new { userId = claimEntity.UserId, id = claimEntity.Id }),
                            Method = "GET"
                        },
                        new Link
                        {
                            Rel = "User",
                            Href = Url.Link("GetUser",  new { id = claimEntity.UserId }),
                            Method = "GET"
                        }
                    }
                } as TDto;
            }

            return null;
        }
    }
}