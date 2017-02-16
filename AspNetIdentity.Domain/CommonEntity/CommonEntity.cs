namespace AspNetIdentity.Domain.CommonEntity
{
    public class CommonEntity<T> : IEntityBase
    {
        public T Id { get; set; }
    }
}