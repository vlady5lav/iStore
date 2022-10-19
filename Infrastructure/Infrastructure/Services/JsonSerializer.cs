using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services;

public class JsonSerializer : IJsonSerializer
{
    public T Deserialize<T>(string value)
    {
        return JsonConvert.DeserializeObject<T>(value)!;
    }

    public string Serialize<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
