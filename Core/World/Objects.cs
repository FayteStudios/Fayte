using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FayteWO.Core.World
{
    public class ObjectDef
    {
        public string defID = "";
        public string defName = "";
        public string spriteID = "";
        public bool stackable;
        public int maxStackSize = 1;
        public bool blocksMovement;
        public bool blocksVision;
        public TileInteractions interactions;

        public void SetID(string id)
        {
            defID = id;
        }
        public string GetID()
        {
            return defID;
        }
        public void SetName(string name)
        {
            defName = name;
        }
        public string GetName()
        {
            return defName;
        }
        public void SetSprite(string sprite)
        {
            spriteID = sprite;
        }
        public string GetSprite()
        {
            return spriteID;
        }
        public void SetStackable(bool can)
        {
            stackable = can;
        }
        public bool Stackable()
        {
            return stackable;
        }
        public void SetMaxStackSize(int size)
        {
            maxStackSize = size;
        }
        public int GetMaxStackSize()
        {
            return maxStackSize;
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
        public void SetInteractions(TileInteractions interact)
        {
            interactions = interact;
        }
        public TileInteractions GetInteractions()
        {
            return interactions;
        }
    }
}