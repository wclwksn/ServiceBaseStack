using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.TilesService
{
    public class tiles
    {
        public int zoom_level { get; set; }

        public int tile_column { get; set; }
        public int tile_row { get; set; }

        public byte[] tile_data { get; set; }
    }
}