using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
//using WebApiContrib.Formatting.Jsonp;

namespace EmployeeService
{

    /*public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
    */

    public static class WebApiConfig
    {
        public static object OAuthDefaults { get; private set; }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication
            
     
            

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //CORS
            EnableCorsAttribute cors = new EnableCorsAttribute("*","*","*");
            config.EnableCors(cors);

            //config.Filters.Add(new RequireHttpsAttribute());

            //JSONP Formatter
            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0,jsonpFormatter);

            //config.Formatters.Add(new CustomJsonFormatter());

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Return JSON Format data
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Return XML Format data
            //config.Formatters.Remove(config.Formatters.JsonFormatter);

            //Formatters : indented and camel case
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        }
    }

    internal class HostAuthenticationFilter : IFilter
    {
        private object oAuthDefaults;

        public HostAuthenticationFilter(object oAuthDefaults)
        {
            this.oAuthDefaults = oAuthDefaults;
        }

        public bool AllowMultiple => throw new System.NotImplementedException();
    }
    
}
