using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebSocketPacket
{
    [JsonProperty("type")] public string Type;
    [JsonProperty("link_id")] public string LinkId;
    [JsonProperty("body")] public Dictionary<string, string> Body;

    public static implicit operator JObject(WebSocketPacket ws) => JObject.FromObject(ws);
}