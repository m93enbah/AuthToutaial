namespace Server.Models
{
    public static class JWTSettings
    {
        public const string Issuer = Audiance;
        public const string Audiance = "https://localhost:44348/";
        public const string Secret = "not_too_short_secret_key_or_otherwise_it_will_Generate_error";
    }
}
