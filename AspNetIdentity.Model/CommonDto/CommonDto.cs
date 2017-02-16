namespace AspNetIdentity.Model.CommonDto
{
    public class CommonDto<T> : DtoBase
    {
        public T Id { get; set; }
    }
}