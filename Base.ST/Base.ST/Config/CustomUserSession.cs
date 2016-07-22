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
           // base.OnAuthenticated(authService, session, tokens, authInfo);

            //Populate all matching fields from this session to your own custom User table
            var user = session.ConvertTo<User>();
            return;
            user.Id = int.Parse(session.UserAuthId);
            

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

            //Resolve the DbFactory from the IOC and persist the user info
            using (var db = authService.TryResolve<IDbConnectionFactory>().Open())
            {
                db.Save(user);
            }
        } 

        private static string CreateGravatarUrl(string email, int size = 64)
        {
            var md5 = MD5.Create();
            var md5HadhBytes = md5.ComputeHash(email.ToUtf8Bytes());

            var sb = new StringBuilder();
            for (var i = 0; i < md5HadhBytes.Length; i++)
                sb.Append(md5HadhBytes[i].ToString("x2"));

            string gravatarUrl = "http://www.gravatar.com/avatar/{0}?d=mm&s={1}".Fmt(sb, size);
            return gravatarUrl;
        }
    }
}