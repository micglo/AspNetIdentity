﻿using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using AspNetIdentity.WebApi.Utility.Filter;
using Newtonsoft.Json.Serialization;

namespace AspNetIdentity.WebApi
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Filters.Add(new ValidateModelFilterAttribute());
        }
    }
}