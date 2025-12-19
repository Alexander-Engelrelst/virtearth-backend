namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class SpawnLocationDto(int xCord, int yCord)
{
    public float X { get; set; } = xCord;
    public float Y { get; set; } = yCord;
}