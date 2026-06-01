using FayteWO.Core.World;

namespace FayteWO.Core.Packets
{
    public enum PacketType
    {
        unknown = 0,

        loginRequest = 100,
        moveRequest = 101,
        chatMessage = 102,
        whisperMessage = 103,
        checkRequest = 104,
        tileDefRequest = 105,
        tileChangeRequest = 106,
        tileInteractRequest = 107,

        loginResult = 200,
        entityMove = 201,
        chunkData = 202,
        serverMessage = 203,
        entitySpawned = 204,
        entityDespawned = 205,
        chatBroadcast = 206,
        whisperReceived = 207,
        tileDef = 208,
        tileMapChunk = 209,
        tileChanged = 210,
        tileObjectSnapshot = 211,
        tileObjectChanged = 212
    }

    //chunks
    public sealed record ChunkDataPacket
    ( ChunkPosition chunkPosition, int size, int height, int[] tileIDs);
    
    public sealed record ChunkRequestPacket
    ( ChunkPosition chunkPosition);










}