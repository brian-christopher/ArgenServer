using System.Text.Json;

namespace Argen.Common;

public class Message
{
    public Opcode Opcode { get; set; }
    public string? Payload { get; set; }

    public void SerializePayload<T>(T data) where T : class
    {
        Payload = JsonSerializer.Serialize(data);
    }

    public T DeserializePayload<T>() where T : class
    {
        return JsonSerializer.Deserialize<T>(Payload!)!;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}