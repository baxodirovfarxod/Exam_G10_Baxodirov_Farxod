using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.RefreshTokenService;

public interface IRefreshTokenService
{
    Task AddRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken> GetRefreshToken(string refreshToken, long userId);
    Task DeleteRefreshToken(string refreshToken);
}