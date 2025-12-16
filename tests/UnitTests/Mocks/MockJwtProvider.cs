using Adria.Application.Contracts;
using Adria.Domain.Users;

namespace UnitTests.Mocks;

public class MockJwtProvider : IJwtProvider
{
    public static readonly string _jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJBbGljZVNtaXRoIiwiZXhwIjoxNzYzMDQyNTM3LCJpc3MiOiJWaXJ0RWFydGggc2VydmVyIiwiYXVkIjoiVmlydEVhcnRoIHBsYXllciJ9.BjgPP2MD37ndrV6agF4mje3U1I0OZhG8J0vIyhmxxuQ";
    public string GenerateToken(User user)
    {
        return _jwtToken;
    }
}