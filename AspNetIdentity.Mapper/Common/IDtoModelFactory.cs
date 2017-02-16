using AspNetIdentity.Domain.CommonEntity;
using AspNetIdentity.Model.CommonDto;

namespace AspNetIdentity.Mapper.Common
{
    public interface IDtoModelFactory
    {
        IEntityBase GetModel(DtoBase dtoModel);
    }
}