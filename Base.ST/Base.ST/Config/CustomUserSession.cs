using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Base.ST
{
    /// <summary>
    /// Create your own strong-typed Custom AuthUserSession where you can add additional AuthUserSession 
    /// fields required for your application. The base class is automatically populated with 
    /// User Data as and when they authenticate with your application. 
    /// </summary>
    public class CustomUserSession : AuthUserSession
    {
        public string CustomId { get; set; }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
           //base.OnAuthenticated(authService, session, tokens, authInfo); 
           //Populate all matching fields from this session to your own custom User table
            var user = session.ConvertTo<User>();

            var authToken = session.ProviderOAuthAccess;
            return;

            //foreach (var authToken in session.ProviderOAuthAccess)
            //{
            //    if (authToken.Provider == FacebookAuthProvider.Name)
            //    {
            //        user.FacebookName = authToken.DisplayName;
            //        user.FacebookFirstName = authToken.FirstName;
            //        user.FacebookLastName = authToken.LastName;
            //        user.FacebookEmail = authToken.Email;
            //    }
            //    else if (authToken.Provider == TwitterAuthProvider.Name)
            //    {
            //        user.TwitterName = user.DisplayName = authToken.UserName;
            //    }
            //    else if (authToken.Provider == YahooOpenIdOAuthProvider.Name)
            //    {
            //        user.YahooUserId = authToken.UserId;
            //        user.YahooFullName = authToken.FullName;
            //        user.YahooEmail = authToken.Email;
            //    }
            //}

            if (AppHost._appConfig.AdminUserNames.Contains(session.UserAuthName)
                && !session.HasRole(RoleNames.Admin))
            {
                var userAuthRepo = authService.TryResolve<IAuthRepository>();
                var userAuth = userAuthRepo.GetUserAuth(session, tokens);
                userAuthRepo.AssignRoles(userAuth, roles: new[] { RoleNames.Admin });
            }

        }
    }
}