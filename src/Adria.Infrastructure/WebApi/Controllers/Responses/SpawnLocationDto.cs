namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class SpawnLocationDto
{
    public float X { get; set; }
    public float Y { get; set; }
    
    public SpawnLocationDto(int xCord, int yCord)
    {
        X = xCord + 0.5f;
        Y = yCord + 0.5f;
    }
}