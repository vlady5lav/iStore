namespace Infrastructure.Services.Interfaces;

public interface IJsonSerializer
{
    T Deserialize<T>(string value);

    string Serialize<T>(T data);
}
