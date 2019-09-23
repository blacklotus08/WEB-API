using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace EmployeeService
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedauthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedauthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if(EmployeeSecurity.Login(username,password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                }

            }
        }
    }
}