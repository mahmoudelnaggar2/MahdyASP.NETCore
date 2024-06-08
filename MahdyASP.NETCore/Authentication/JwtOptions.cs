public class JwtOptions
{
    public string Issuer { set; get; }
    public string Audiance { set; get; }
    public int Lifetime { set; get; }
    public string SigningKey { set; get; }
}