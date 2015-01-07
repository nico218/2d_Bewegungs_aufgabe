using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileMaps
{
    class Tile
    {
        public enum Types { Path, Water };

        public Types Type { get; private set; }

        public Tile(Types type)
        {
            this.Type = type;
        }
    }
}
