﻿using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ServiceStack.OrmLite;
using Base.ST.DomainModel;

namespace Base.ST
{
    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public CustomCredentialsAuthProvider()
        {
        }

        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            using (var db = authService.TryResolve<IDbConnectionFactory>().Open())
            {
                var _exUser = db.Single<sms_user>(x => x.name == userName && x.password == password);
                if (_exUser == null)
                {
                    return false;
                }
                var session = (CustomUserSession)authService.GetSession(false);
                session.UserAuthId = _exUser.userid;
                session.IsAuthenticated = true;  
                sms_loginlog _loginLog = new sms_loginlog() { id = Guid.NewGuid().ToString(), createdate = session.CreatedAt.ToLocalTime(), sessionid = session.Id, status = session.State, ipaddress = string.Empty, ipaddressname = string.Empty, username = userName };
                db.Insert(_loginLog);
            } 
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