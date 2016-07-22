using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Funq;
using System.Reflection;
using ServiceStack.Text;
using ServiceStack.Configuration;
using ServiceStack.Caching;
using ServiceStack.Auth;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.FluentValidation;
using Base.ST.ServiceInterface;

namespace Base.ST
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("BaseST", typeof(HelloWorldService).Assembly) { }

        public static AppConfig _appConfig;

        public override void Configure(Container container)
        {  
            JsConfig.EmitCamelCaseNames = true;
            var appSettings = new AppSettings();
            _appConfig = new AppConfig(appSettings); 
            container.Register<ICacheClient>(new MemoryCacheClient()); 
            //container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("localhost:6379"));
             
            this.ConfigureAuth(container, appSettings);
             
            //const Feature disableFeatures = Feature.Jsv | Feature.Soap;
            SetConfig(new HostConfig
            {
                //EnableFeatures = Feature.All.Remove(disableFeatures),
                AppendUtf8CharsetOnContentTypes = new HashSet<string> { MimeTypes.Html },
            });

            Plugins.Add(new CorsFeature());
        }

        private void ConfigureAuth(Funq.Container container, IAppSettings appSettings)
        {
            //Enable and register existing services you want this host to make use of. 
            SetConfig(new HostConfig
            {
                DebugMode = appSettings.Get("DebugMode", false),
            });

            //Register all Authentication methods you want to enable for this web app.            
            Plugins.Add(new AuthFeature(
                () => new CustomUserSession(),
                new IAuthProvider[] {             
                    new CustomCredentialsAuthProvider() {  SessionExpiry =TimeSpan.FromMinutes(10)},
                    new BasicAuthProvider(),                   
                    new OpenIdOAuthProvider(appSettings)     
                }));

            //Create a DB Factory configured to access the UserAuth PostgreSQL DB
            var connStr = appSettings.GetString("ConnectionString");
            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(connStr, //ConnectionString in Web.Config
                    PostgreSqlDialect.Provider)
                {
                    //ConnectionFilter = x => new ProfiledDbConnection(x, ServiceStack.MiniProfiler.Profiler.Current)
                });
            Plugins.Add(new RequestLogsFeature());
        }
    }
}