namespace Kata.Presentation.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }
}