namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class CoordinatesDto(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}