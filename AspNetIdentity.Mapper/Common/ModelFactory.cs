using System.Web.Http.Routing;
using AspNetIdentity.WebApi.Utility.RequestMessageProvider;

namespace AspNetIdentity.Mapper.Common
{
    public abstract class ModelFactory
    {
        protected UrlHelper Url { get; set; }
        protected ModelFactory(IRequestMessageProvider requestMessageProvider)
        {
            if(requestMessageProvider.CurrentMessage != null)
                Url = new UrlHelper(requestMessageProvider.CurrentMessage);
        }

        protected bool TypesEqual<T1, T2>()
            => typeof(T1) == typeof(T2);

        protected bool TypesEqual<T>(object model)
            => model.GetType() == typeof(T);
    }
}