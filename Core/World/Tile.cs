using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FayteWO.Core.World
{
    public enum Direction
    {
        north = 0,
        east = 1,
        south = 2,
        west = 3
    }
    public enum TileFlags
    {
        none = 0,
        blocksMovement = 1,
        blocksSight = 2,
        water = 4,
        resource = 8,
        indoor =  16,
        road = 32
    }
    [Flags]
    public enum TileInteractions
    {
        none = 0,
        walk = 1,
        examine = 2,
        pickup = 4,
        drop = 8,
        use = 16,
        open = 32,
        close = 64,
        gather = 128,
        attack = 256
    }
    public class DirectionExtensions
    {
        public static TilePosition ToOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.north:
                    return new TilePosition(0, -1, 0);

                case Direction.east:
                    return new TilePosition(1, 0, 0);

                case Direction.south:
                    return new TilePosition(0, 1, 0);

                case Direction.west:
                    return new TilePosition(-1, 0, 0);

                default:
                    return TilePosition.zero;
            }
        }
    }
    public class WorldTile
    {
        public int x;
        public int y;
        public int z;
        public int ID;
        public string groundSpriteID = "";
        public List<string> overlaySpriteIDs = new();
        public bool blocksMovement;
        public bool blocksVision;
        public TileInteractions interactions;
        public TileStack objects = new();
        public WorldTile(int _x, int _y, int _z, int tileID)
        {
            x = _x;
            y = _y;
            z = _z;
            ID = tileID;
        }

        public void SetX(int _x)
        {
            x = _x;
        }
        public int GetX()
        {
            return x;
        }
        public void SetY(int _y)
        {
            y = _y;
        }
        public int GetY()
        {
            return y;
        }
        public void SetZ(int _z)
        {
            x = _z;
        }
        public int GetZ()
        {
            return z;
        }
        public void SetTileID(int id)
        {
            ID = id;
        }
        public int GetTileID()
        {
            return ID;
        }
        public void SetGroundSprite(string sprite)
        {
            groundSpriteID = sprite;
        }
        public string GetGroundSprite()
        {
            return groundSpriteID;
        }
        public void SetOverlaySprites(List<string> ids)
        {
            overlaySpriteIDs = ids;
        }
        public void AddOverlaySprite(string id)
        {
            overlaySpriteIDs.Add(id);
        }
        public string GetOverlaySprite(int index)
        {
            if(index >= 0 && index < overlaySpriteIDs.Capacity)
            {
                return overlaySpriteIDs[index];
            }
            return "";
        }
        public void ChangeOverlaySprite(int index, string id)
        {
            if(index >= 0 && index < overlaySpriteIDs.Count)
            {
                overlaySpriteIDs[index] = id;
            }
        }
        public void RemoveOverlaySprite(int index)
        {
            if(index >= 0 && index < overlaySpriteIDs.Count)
            {
                overlaySpriteIDs.RemoveAt(index);
            }
        }
        public void SetBlocksMovement(bool x)
        {
            blocksMovement = x;
        }
        public bool GetBlocksMovement()
        {
            return blocksMovement;
        }
        public void SetBlocksVision(bool x)
        {
            blocksVision = x;
        }
        public bool GetBlocksVison()
        {
            return blocksVision;
        }
        public TileInteractions Interactions(TileInteractions interact)
        {
            switch(interact)
            {
                case TileInteractions.walk:
                    return TileInteractions.walk;
                case TileInteractions.examine:
                    return TileInteractions.examine;
                case TileInteractions.pickup:
                    return TileInteractions.pickup;
                case TileInteractions.drop:
                    return TileInteractions.drop;
                case TileInteractions.use:
                    return TileInteractions.use;
                case TileInteractions.open:
                    return TileInteractions.open;
                case TileInteractions.close:
                    return TileInteractions.close;
                case TileInteractions.gather:
                    return TileInteractions.gather;
                case TileInteractions.attack:
                    return TileInteractions.attack;
                default:
                    return TileInteractions.none;
            }
        }
        public TileStack Objects()
        {
            return objects;
        }
    }
    public class Tile
    {
        public int tileID;
        public string tileName = "";
        public TileFlags flags;
        public Tile(int id, string name, TileFlags flags)
        {
            tileID = id;
            tileName = name;
            this.flags = flags;
        }

        public int GetID()
        {
            return tileID;
        }
        public string GetName()
        {
            return tileName;
        }
        public TileFlags GetFlags()
        {
            return flags;
        }
        public bool BlocksMovement()
        {
            if(flags.HasFlag(TileFlags.blocksMovement))
                return true;
            return false;
        }
        public bool BlocksSight()
        {
            if(flags.HasFlag(TileFlags.blocksSight))
                return true;
            return false;
        }
        public override string ToString()
        {
            return $"{tileName} [{tileID}]"; 
        }   
    }

    public class TileDef
    {
        public int tileID;
        public string tileName = "";
        public string groundSprite = "";
        public List<string> defaultOverlapSprite = new();
        public bool blocksMovement;
        public bool blocksVision;
        public TileInteractions Interactions;
    }



    public record class TilePosition(int x, int y, int z)
    {
        public static TilePosition zero = new(0,0,0);

        public TilePosition Offset(int _x, int _y, int _z)
        {
            TilePosition _offset = new TilePosition(x + _x, y + _y, z + _z);
            return _offset;
        }
        public TilePosition Offset(Direction direction)
        {
            switch (direction)
            {
                case Direction.north:
                    return new TilePosition(x, y - 1, z);

                case Direction.east:
                    return new TilePosition(x + 1, y, z);

                case Direction.south:
                    return new TilePosition(x, y + 1, z);

                case Direction.west:
                    return new TilePosition(x -1, y, z);

                default:
                    return TilePosition.zero;
            }
        }
        public int DistanceTo(TilePosition other)
        {
           return (int)(MathF.Abs(x - other.x) + MathF.Abs(y - other.y) + MathF.Abs(z - other.z));
 
        }
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }

    public class TileStackEntry
    {
        public string objectDefID = "";
        public int quantity = 1;
        public int sortOrder;
        public bool blocksMovement;
        public bool blocksVision;
        public TileInteractions interactions;
        public TileStackEntry clone()
        {
            TileStackEntry newEntry = new TileStackEntry();
            objectDefID = GetObjectDef();
            quantity = GetQuantity();
            blocksMovement = BlocksMovement();
            blocksVision = BlocksVision();
            interactions = Interactions();
            return newEntry;
        }

        public void SetObjectDef(string id)
        {
            objectDefID = id;
        }
        public string GetObjectDef()
        {
            return objectDefID;
        }
        public void SetQuantity(int x)
        {
            quantity = x;
        }
        public void ChangeQuantity(int x)
        {
            quantity += x;
        }
        public int GetQuantity()
        {
            return quantity;
        }
        public void SetBlocksMovement(bool can)
        {
            blocksMovement = can;
        }
        public bool BlocksMovement()
        {
            return blocksMovement;
        }
        public void SetBlocksVision(bool can)
        {
            blocksVision = can;
        }
        public bool BlocksVision()
        {
            return blocksVision;
        }
        public TileInteractions Interactions()
        {
            return interactions;
        }

        public void SetSortOrder(int i)
        {
            sortOrder = i;
        }
        public int GetSortOrder()
        {
            return sortOrder;
        }
    }

    public class TileStack
    {
        public const int DefaultMaxObjectsPerTile = 10;
        private List<TileStackEntry> objects = new();
        public int maxObjects;
        public TileStack(int maxObjects = DefaultMaxObjectsPerTile)
        {
            this.maxObjects = maxObjects;
        }
        public bool CanAddObject()
        {
            bool can = objects.Count < maxObjects;
            return can;
        }
        public bool TryToAddObject(TileStackEntry entry)
        {
            if(!CanAddObject()) return false;

            objects.Add(entry);
            return true;
        }
        public void RemoveObject(TileStackEntry entry)
        {
            if(objects.Contains(entry))
            {
                objects.Remove(entry);
            }
        }
        public bool BlocksMovement()
        {
            foreach(TileStackEntry entry in objects)
            {
                if(entry.blocksMovement)
                return true;
            }
            return false;
        }
        public bool BlocksVision()
        {
            foreach(TileStackEntry entry in objects)
            {
                if(entry.blocksVision)
                return true;
            }
            return false;
        }
        public TileInteractions GetInteractions()
        {
            TileInteractions interactions = TileInteractions.none;
            foreach(TileStackEntry entry in objects)
            {
                interactions |= entry.Interactions();
            }
            return interactions;
        }
        public int GetMaxObjects()
        {
            return maxObjects;
        }
        public int StackCount()
        {
            return objects.Count;
        }

        public TileStack Clone()
        {
            TileStack clone = new(maxObjects);
            foreach(TileStackEntry item in objects)
            {
                clone.TryToAddObject(item.clone());
            }
            return clone;
        }
    }


    public class TileFactory
    {
        public static WorldTile CreateFromDef(int x, int y, int z, TileDef def)
        {
            return new WorldTile(x, y, z, def.tileID)
            {
                groundSpriteID = def.groundSprite,
                overlaySpriteIDs = new List<string>(def.defaultOverlapSprite),
                blocksMovement = def.blocksMovement,
                blocksVision = def.blocksVision,
                interactions = def.Interactions
            };
        }
        public static TileStackEntry CreateStackEntry(ObjectDef def, int quantity = 1)
        {
            int finalQuantity = Math.Max(1, quantity);
            if(!def.Stackable()) finalQuantity = 1;
            else if(def.maxStackSize > 0) finalQuantity = Math.Min(finalQuantity, def.maxStackSize);
            return new TileStackEntry
            {
                objectDefID = def.defID,
                quantity = finalQuantity,
                blocksMovement = def.blocksMovement,
                blocksVision = def.blocksVision,
                interactions = def.interactions
            };
        }
    }
}