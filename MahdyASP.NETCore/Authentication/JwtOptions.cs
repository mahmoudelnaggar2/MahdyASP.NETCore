namespace MahdyASP.NETCore.Authentication;

public record JwtOptions(string Issuer, string Audiance, int Lifetime, string SigningKey);