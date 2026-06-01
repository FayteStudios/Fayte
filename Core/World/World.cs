
using FayteWO.Core.World;

namespace FayteWo.Core.World
{
    public class Catalog
    {
        public Dictionary<int, TileDef> tileDefs = new();
        public Dictionary<string, ObjectDef> objectDefs = new();
        
        public Catalog CreateCatalog()
        {
            //will need to be handled with jsons later
            Catalog catalog = new();
            catalog.RegisterTile(new TileDef
            {
                tileID = 0,
                tileName = "Blank",
                blocksMovement = false,
                blocksVision = false,
                Interactions = TileInteractions.examine
            });
            catalog.RegisterTile(new TileDef
            {
                tileID = 1,
                tileName = "Grass",
                blocksMovement = false,
                blocksVision = false,
                Interactions = TileInteractions.examine
            });
            catalog.RegisterTile(new TileDef
            {
                tileID = 2,
                tileName = "Test Wall",
                blocksMovement = false,
                blocksVision = false,
                Interactions = TileInteractions.examine
            });
            catalog.RegisterTile(new TileDef
            {
                tileID = 3,
                tileName = "Water",
                blocksMovement = false,
                blocksVision = false,
                Interactions = TileInteractions.examine
            });
            catalog.RegisterObject(new ObjectDef
            {
                defID = "item.coin",
                defName = "Coin",
                spriteID = "item.coin.001",
                stackable = true,
                maxStackSize = 9999,
                blocksMovement = false,
                blocksVision = false,
                interactions = TileInteractions.pickup | TileInteractions.examine
            });
            catalog.RegisterObject(new ObjectDef
            {
                defID = "item.WoodenWall",
                defName = "Wooden Wall",
                spriteID = "item.woodenWall.001",
                stackable = false,
                maxStackSize = 1,
                blocksMovement = true,
                blocksVision = true,
                interactions = TileInteractions.examine
            });
            return catalog;
        }

        public void RegisterTile(TileDef def)
        {
            tileDefs[def.tileID] = def;
        }
        public void RegisterObject(ObjectDef def)
        {
            objectDefs[def.defID] = def;
        }
        public bool TryGetTile(int id, out TileDef? def)
        {
            return tileDefs.TryGetValue(id, out def);
        }
        public bool TryGetObject(string id, out ObjectDef def)
        {
            return objectDefs.TryGetValue(id, out def);
        }
    }

    public class WorldMap
    {
        public Dictionary<ChunkPosition, Chunk> chunks = new();
        public Dictionary<TilePosition, TileStack> tileObjectStacks = new();
        
        public void AddChunk(Chunk chunk)
        {
            ChunkPosition position = new ChunkPosition(chunk.chunkX, chunk.chunkY, chunk.chunkZ);
            chunks.Add(position, chunk);
        }

        public bool TryGetChunk(ChunkPosition position, out Chunk? chunk)
        {
            return chunks.TryGetValue(position, out chunk);
        }
        public bool TryGetChunkPosition(TilePosition pos, out Chunk? chunk)
        {
            ChunkPosition position = ChunkPosition.WorldPosition(pos);
            return TryGetChunk(position, out chunk);
        }
        public bool TryGetTileID(TilePosition pos, out int tileID)
        {
            tileID = 0;
            if(!TryGetChunkPosition(pos, out Chunk? chunk) || chunk is null) return false;
            int _x = Chunk.ChunkCoordinate(pos.x);
            int _y = Chunk.ChunkCoordinate(pos.y);
            int _z = pos.z - chunk.chunkZ;

            if(!chunk.ContainsPosition(_x, _y, _z)) return false;

            tileID = chunk.GetID(_x, _y, _z);
            return true;
        }
        public bool TryGetWorldTile(TilePosition pos, Catalog catalog, out WorldTile? tile)
        {
            tile = null;
            if(!TryGetTileID(pos, out int tileID)) return false;

            if(catalog.TryGetTile(tileID, out TileDef? def) && def is not null)
            {
                tile = TileFactory.CreateFromDef(pos.x, pos.y, pos.z, def);
            }
            if(tileObjectStacks.TryGetValue(pos, out TileStack? stack))
            {
                if(tile != null) tile.objects = stack.Clone();
            }
            return true;
        }
        public bool TrySetTile(TilePosition pos, int id)
        {
            if(!TryGetChunkPosition(pos, out Chunk? chunk)) return false;
        }
    }

}