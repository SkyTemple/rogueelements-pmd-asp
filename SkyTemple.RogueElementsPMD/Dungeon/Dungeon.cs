using System.Text;
using RogueElements;

namespace SkyTemple.RogueElementsPMD.Dungeon
{
    public class Dungeon
    {
        public static string Generate(int cellX, int cellY, int cellWidth=10, int cellHeight=10)
        {
            var layout = new MapGen<MapGenContext>();

            
            var startGen = new InitGridPlanStep<MapGenContext>(1)
            {
                CellX = cellX,
                CellY = cellY,
                CellWidth = cellWidth - 1,
                CellHeight = cellHeight - 1,
            };
            layout.GenSteps.Add(-4, startGen);

            // Create a path that is composed of a ring around the edge
            var path = new GridPathBranch<MapGenContext>
            {
                RoomRatio = new RandRange(70),
                BranchRatio = new RandRange(0, 50),
            };

            var genericRooms = new SpawnList<RoomGen<MapGenContext>>
            {
                {new RoomGenSquare<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 15}
            };
            path.GenericRooms = genericRooms;

            var genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>
            {
                {new RoomGenAngledHall<MapGenContext>(0), 10},
            };
            path.GenericHalls = genericHalls;

            layout.GenSteps.Add(-4, path);

            // Output the rooms into a FloorPlan
            layout.GenSteps.Add(-2, new DrawGridToFloorStep<MapGenContext>());

            // Draw the rooms of the FloorPlan onto the tiled map, with 1 TILE padded on each side
            layout.GenSteps.Add(0, new DrawFloorToTileStep<MapGenContext>(1));

            // Generate water (specified by user as Terrain 2) with a frequency of 20%, using Perlin Noise in an order of 3, softness 1.
            const int terrain = 2;
            var waterPostProc = new PerlinWaterStep<MapGenContext>(new RandRange(20), 3, new Tile(terrain), 1, false);
            layout.GenSteps.Add(3, waterPostProc);

            // Remove walls where diagonals of water exist and replace with water
            layout.GenSteps.Add(4, new DropDiagonalBlockStep<MapGenContext>(new Tile(terrain)));

            // Remove water stuck in the walls
            layout.GenSteps.Add(4, new EraseIsolatedStep<MapGenContext>(new Tile(terrain)));

            // Run the generator and print
            MapGenContext context = layout.GenMap(MathUtils.Rand.NextUInt64());
            return Format(context.Map);
        }

        private static string Format(Map map)
        {
            var topString = new StringBuilder(string.Empty);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    char tileChar;
                    Tile tile = map.Tiles[x][y];
                    switch (tile.ID)
                    {
                        case Map.WALL_TERRAIN_ID:
                            tileChar = '#';
                            break;
                        case Map.ROOM_TERRAIN_ID:
                            tileChar = '.';
                            break;
                        case Map.WATER_TERRAIN_ID:
                            tileChar = '~';
                            break;
                        default:
                            tileChar = '?';
                            break;
                    }

                    topString.Append(tileChar);
                }

                topString.Append('\n');
            }

            return topString.ToString();
        }
    }
}