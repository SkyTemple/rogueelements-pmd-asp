using System;
using RogueElements;

namespace SkyTemple.RogueElementsPMD.Dungeon
{
    public class MapGenContext : ITiledGenContext, IRoomGridGenContext
    {
        public MapGenContext()
        {
            this.Map = new Map();
        }

        public Map Map { get; set; }

        public ITile RoomTerrain => new Tile(RogueElementsPMD.Dungeon.Map.ROOM_TERRAIN_ID);

        public ITile WallTerrain => new Tile(RogueElementsPMD.Dungeon.Map.WALL_TERRAIN_ID);

        public bool TilesInitialized => this.Map.Tiles != null;

        public int Width => this.Map.Width;

        public int Height => this.Map.Height;

        public IRandom Rand => this.Map.Rand;

        public ITile GetTile(Loc loc) => this.Map.Tiles[loc.X][loc.Y];

        public virtual bool CanSetTile(Loc loc, ITile tile) => true;

        public bool TrySetTile(Loc loc, ITile tile)
        {
            if (!this.CanSetTile(loc, tile))
                return false;
            this.Map.Tiles[loc.X][loc.Y] = (Tile)tile;
            return true;
        }

        public void SetTile(Loc loc, ITile tile)
        {
            if (!this.TrySetTile(loc, tile))
                throw new InvalidOperationException("Can't place tile!");
        }

        public void InitSeed(ulong seed)
        {
            this.Map.Rand = new ReRandom(seed);
        }

        bool ITiledGenContext.TileBlocked(Loc loc)
        {
            return this.Map.Tiles[loc.X][loc.Y].ID == RogueElementsPMD.Dungeon.Map.WALL_TERRAIN_ID;
        }

        bool ITiledGenContext.TileBlocked(Loc loc, bool diagonal)
        {
            return this.Map.Tiles[loc.X][loc.Y].ID == RogueElementsPMD.Dungeon.Map.WALL_TERRAIN_ID;
        }

        public virtual void CreateNew(int width, int height)
        {
            this.Map.InitializeTiles(width, height);
        }

        public virtual void FinishGen()
        {
        }

        public FloorPlan RoomPlan { get; private set; }

        public GridPlan GridPlan { get; private set; }

        public void InitPlan(FloorPlan plan)
        {
            this.RoomPlan = plan;
        }

        public void InitGrid(GridPlan plan)
        {
            this.GridPlan = plan;
        }
    }
}
