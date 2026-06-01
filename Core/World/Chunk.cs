
using System.Runtime.CompilerServices;

namespace FayteWO.Core.World
{
    public class Chunk
    {
        public const int size = 32;
        private int[,,] tileIDs;
        public int chunkX;
        public int chunkY;
        public int chunkZ;
        public static int worldDepth = 5;

        public Chunk(int x, int y, int z)
        {
            chunkX = x;
            chunkY = y;
            chunkZ = z;

            tileIDs = new int[size, size, worldDepth];
        }

        public int GetX()
        {
            return chunkX;
        }
        public int GetY()
        {
            return chunkY;
        }
        public int GetZ()
        {
            return chunkZ;
        }
        public int GetID(int _x, int _y, int _z)
        {
            ValidatePosition(_x, _y, _z);
            return tileIDs[_x, _y, _z];
        }
        public void SetID(int _x, int _y, int id, int _z = 0)
        {
            ValidatePosition(_x, _y, _z);
            tileIDs[_x, _y, _z] = id;
        }
        public void ValidatePosition(int x, int y, int z)
        {
            if(ContainsPosition(x, y, z))
            {
                //throws an error
            }
        }
        public bool ContainsPosition(int _x, int _y, int _z)
        {
            if(_x >= 0 && _x < size && _y >= 0 && _y < size && _z >= 0 && _z < worldDepth)
            {
                return true;
            }
            return false;
        }
        public TilePosition GetChunkPosition(int x, int y, int z)
        {
            ValidatePosition(x, y, z);

            int _X = chunkX * size + x;
            int _y = chunkY * size + y;
            int _z = chunkZ + z;
            return new TilePosition(_X, _y, _z);
        }
        public static int ChunkCoordinate(int worldCoordinate)
        {
            int quotient = Math.DivRem(worldCoordinate, size, out int remainder);

            if (worldCoordinate >= 0 || remainder == 0)
            {
                return quotient;
            }
            else
            {
                return quotient - 1;
            }
        }
        public int LocalCoord(int coord)
        {
            int local = coord % size;
            if(local < 0)
                local += size;
            return local;
        }
        public int[] ToTileIDArray()
        {
            int[] IDs = new int[size * size * worldDepth];
            int index = 0;
            for(int z = 0; z < worldDepth; z++)
            {
                for(int y = 0; y < size; y++)
                {
                    for(int x = 0; x < size; x++)
                    {
                        IDs[index] = tileIDs[x, y, z];
                        index++;
                    }
                }
            }
            return IDs;
        }
        public void LoadTileIDArray(int[] _tileIDs)
        {
            int length = size * size * worldDepth;
            
            int index = 0;
            for(int z = 0; z < worldDepth; z++)
            {
                for(int y = 0; y < size; y++)
                {
                    for(int x = 0; x < size; x++)
                    {
                        int ID = _tileIDs[index];
                        tileIDs[x, y, z] = ID;
                        index++;
                    }
                }
            }
        }
    }


    public readonly record struct ChunkPosition(int X, int Y, int Z)
    {
        public static ChunkPosition WorldPosition(TilePosition position)
        {
            int chunkX = Chunk.ChunkCoordinate(position.x);
            int chunky = Chunk.ChunkCoordinate(position.y);
            int chunkz = Chunk.ChunkCoordinate(position.z);

            return new ChunkPosition(chunkX, chunky, chunkz);
        }
    }


    public record ChunkRequestPacket(ChunkPosition chunkPosition);

}