using RogueElements;

namespace SkyTemple.RogueElementsPMD.Dungeon
{
    public class Map
    {
        public const int WALL_TERRAIN_ID = 0;
        public const int ROOM_TERRAIN_ID = 1;
        public const int WATER_TERRAIN_ID = 2;

        public ReRandom Rand { get; set; }

        public Tile[][] Tiles { get; set; }

        public int Width => this.Tiles.Length;

        public int Height => this.Tiles[0].Length;

        public void InitializeTiles(int width, int height)
        {
            this.Tiles = new Tile[width][];
            for (int ii = 0; ii < width; ii++)
            {
                this.Tiles[ii] = new Tile[height];
                for (int jj = 0; jj < height; jj++)
                    this.Tiles[ii][jj] = new Tile();
            }
        }
    }
}
