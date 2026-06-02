using System.Text.Json;
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

    public sealed record NetworkPacket(PacketType type, JsonElement payload);

    //chunks
    public sealed record ChunkDataPacket(ChunkPosition chunkPosition, int size, int height, int[] tileIDs);
    
    public sealed record ChunkRequestPacket(ChunkPosition chunkPosition);

    public sealed record ChatBroadcastPacket(Guid senderID, string senderName, string message);

    public sealed record ChatMessagePacket(string message);

    public sealed record EntitySpawnedPacket(Guid entityID, string name, TilePosition position);

    public sealed record EntityDespawnedPacket(Guid entityID, string reason);

    public sealed record EntityMovedPacket(Guid entityID, TilePosition fromPosition, TilePosition toPosition, Direction direction);

    public sealed record LoginRequestPacket(string username, string password);

    public sealed record LoginResultPacket(bool success, string message, Guid? playerID, TilePosition? spawnPosition);

    public sealed record MoveRequestPacket(Direction direction);

    public sealed record ServerMessagePacket(string message);

    public sealed record TileChangedPacket(TilePosition position, int id);

    public sealed record TileChangeRequestPacket(TilePosition pos, int id);

    public sealed record TileDefinitionsPacket(List<TileDef> tiles);

    public sealed record TileDefRequestPacket;

    public sealed record TileInteractionRequestPacket(TilePosition position);

    public sealed record WhisperMessagePacket(string targetUser, string message);

    public sealed record WhisperReceivedPacket(Guid sender, string senderName, string targetName, string message, bool outgoingCopy);


    public class PacketSerializer
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true, WriteIndented = false
        };

        public static string Serialize<TPayload>(PacketType packetType, TPayload payload)
        {
            JsonElement payloadElement = JsonSerializer.SerializeToElement(payload, JsonOptions);
            NetworkPacket packet = new(packetType, payloadElement);
            return JsonSerializer.Serialize(packet, JsonOptions);
        }

        public static NetworkPacket DeserializeEnvelope(string json)
        {
            NetworkPacket? packet = JsonSerializer.Deserialize<NetworkPacket>(json, JsonOptions);
            if(packet is not null) return packet;
            else throw new InvalidOperationException("Failed to deserialize packet.");
        }

        public static TPayload DeserializePayload<TPayload>(NetworkPacket packet)
        {
            TPayload? payload = packet.payload.Deserialize<TPayload>(JsonOptions);
            if(payload is not null) return payload;
            else throw new InvalidOperationException("Failed to deserialize payload");
        }

    }




}