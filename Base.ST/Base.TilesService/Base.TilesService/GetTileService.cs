 
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Base.TilesService
{
    public class GetTileService : Service
    {
        [AddHeader(ContentType = "image/png")]
        public Stream Get(tileInfo _tileInfo)
        {
               Stream stream = new MemoryStream();
               tiles _singleTile = Db.Single<tiles>(parm => parm.zoom_level == _tileInfo.z && parm.tile_column == _tileInfo.x && parm.tile_row == _tileInfo.y);
               if (_singleTile != null)
               {
                   stream = new MemoryStream(_singleTile.tile_data);
               }

               return stream;
        }
    } 

    [Route("/GetTile/{z}/{x}/{y}", "GET")]
    public class tileInfo 
    {
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
