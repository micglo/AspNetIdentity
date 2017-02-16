using AspNetIdentity.Domain.CommonEntity;
using AspNetIdentity.Model.CommonDto;

namespace AspNetIdentity.Mapper.Common
{
    public interface IDomainModelFactory
    {
        TDto GetModel<TDto>(IEntityBase domainEntity) where TDto : DtoBase;
    }
}