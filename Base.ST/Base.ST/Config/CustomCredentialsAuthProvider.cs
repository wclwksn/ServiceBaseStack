using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Base.ST
{
    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public CustomCredentialsAuthProvider()
        {
        }

        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        { 
            //if (!Membership.ValidateUser(userName, password)) return false;

            var session = (CustomUserSession)authService.GetSession(false);
            Console.Write(session.Id.ToString());
            session.UserAuthId = "2";
            session.IsAuthenticated = true; 

            // add roles 
            session.Roles = new List<string>();
            if (session.UserAuthId == "admin") session.Roles.Add(RoleNames.Admin);
            session.Roles.Add("User"); 

            return true;
        }

        public override IHttpResult OnAuthenticated(IServiceBase authService,
            IAuthSession session, IAuthTokens tokens,
            Dictionary<string, string> authInfo)
        { 
             return base.OnAuthenticated(authService, session, tokens, authInfo);  
        }

       
    }
}