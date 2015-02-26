using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        public CustomPrincipal GetUser()
        {
            return HttpContext.Current.User as CustomPrincipal;
        }

        public void Login(string userName, bool isPersistent, string customData, int timeout)
        {
            FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(
                                                     1,
                                                     userName,
                                                     DateTime.Now,
                                                     DateTime.Now.AddMinutes(timeout),
                                                     isPersistent,
                                                     customData);

            var encryptTicket = FormsAuthentication.Encrypt(authenticationTicket);
            var authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
            if (isPersistent)
                authenticationCookie.Expires = authenticationTicket.Expiration;

            HttpContext.Current.Response.Cookies.Add(authenticationCookie);
        }

        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public void PostAuthenticateRequest()
        {
            var authenticationCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authenticationCookie != null)
            {
                FormsAuthenticationTicket authenticationTicket = FormsAuthentication.Decrypt(authenticationCookie.Value);

                var serializer = new JavaScriptSerializer();

                var serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authenticationTicket.UserData);

                CustomPrincipal user = new CustomPrincipal(authenticationTicket.Name);
                user.Id = serializeModel.Id;
                user.Nome = serializeModel.Nome;
                user.Login = serializeModel.Login;
                user.Roles = serializeModel.Roles;

                HttpContext.Current.User = System.Threading.Thread.CurrentPrincipal = user;
            }
        }
    }
}