using System.Net.Http;

namespace AspNetIdentity.WebApi.Utility.RequestMessageProvider
{
    public interface IRequestMessageProvider
    {
        HttpRequestMessage CurrentMessage { get; }
    }
}