using Funq;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Base.TilesService
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Base.TilesService", typeof(GetTileService).Assembly) { }

        public override void Configure(Container container)
        {
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(@"D:\Program Files\ArcTiler\Desktop\4.7\Output\nantongrastermap_瓦片包：MBTiles\nantongrastermap.mbtiles", SqliteDialect.Provider));
            container.RegisterAs<OrmLiteCacheClient, ICacheClient>();
            container.Resolve<ICacheClient>().InitSchema();

            //using (var db = container.Resolve<IDbConnectionFactory>().Open())
            //{
            //    List<tiles> _listTiles = db.Select<tiles>();
            //}
        }
    }
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}