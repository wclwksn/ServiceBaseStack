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
            //Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;
            var appSettings = new AppSettings();
            _appConfig = new AppConfig(appSettings);

            //Register a external dependency-free 
            container.Register<ICacheClient>(new MemoryCacheClient());
            //Configure an alt. distributed persistent cache that survives AppDomain restarts. e.g Redis
            //container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("localhost:6379"));

            //Enable Authentication an Registration
            ConfigureAuth(container, appSettings);

            //Change the default ServiceStack configuration
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
            //Look in Web.config for examples on how to configure your oauth providers, e.g. oauth.facebook.AppId, etc. 
            SetConfig(new HostConfig
            {
                DebugMode = appSettings.Get("DebugMode", false),
            });

            //Register all Authentication methods you want to enable for this web app.            
            Plugins.Add(new AuthFeature(
                () => new CustomUserSession(), //Use your own typed Custom UserSession type
                new IAuthProvider[] {             //HTML Form post of UserName/Password credentials    
                    new CustomCredentialsAuthProvider() {  SessionExpiry =TimeSpan.FromMinutes(15)},
                    new BasicAuthProvider(),                    //Sign-in with Basic Auth 
                    new OpenIdOAuthProvider(appSettings)      //Sign-in with Custom OpenId
                }));  

            //Create a DB Factory configured to access the UserAuth PostgreSQL DB
            var connStr = appSettings.GetString("ConnectionString");
            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(connStr, //ConnectionString in Web.Config
                    PostgreSqlDialect.Provider)
                {
                    //ConnectionFilter = x => new ProfiledDbConnection(x, ServiceStack.MiniProfiler.Profiler.Current)
                });

            //Store User Data into the referenced   database
            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>())); //Use OrmLite DB Connection to persist the UserAuth and AuthProvider info

            var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>(); //If using and RDBMS to persist UserAuth, we must create required tables
            if (appSettings.Get("RecreateAuthTables", false))
            {
                authRepo.DropAndReCreateTables(); //Drop and re-create all Auth and registration tables
            }
            else
            {
                authRepo.InitSchema();   //Create only the missing tables
            }

            Plugins.Add(new RequestLogsFeature());
        }
    }
}