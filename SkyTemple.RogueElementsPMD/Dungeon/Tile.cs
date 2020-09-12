using RogueElements;

namespace SkyTemple.RogueElementsPMD.Dungeon
{
    public class Tile : ITile
    {
        public Tile()
        {
            this.ID = Map.WALL_TERRAIN_ID;
        }

        public Tile(int id)
        {
            this.ID = id;
        }

        protected Tile(Tile other)
        {
            this.ID = other.ID;
        }

        public int ID { get; set; }

        public ITile Copy() => new Tile(this);

        public bool TileEquivalent(ITile other)
        {
            if (!(other is Tile tile))
                return false;
            return tile.ID == this.ID;
        }
    }
}
