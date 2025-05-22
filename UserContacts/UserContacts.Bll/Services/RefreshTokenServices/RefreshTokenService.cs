using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContacts.Core.Errors;
using UserContacts.Dal;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.RefreshTokenService;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly MainContext mainContext;

    public RefreshTokenService(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }

    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await mainContext.RefreshTokens.AddAsync(refreshToken);
        await mainContext.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        var token = await mainContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (token == null)
        {
            throw new EntityNotFoundException();
        }

        mainContext.RefreshTokens.Remove(token);
        await mainContext.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetRefreshToken(string refreshToken, long userId)
    {
        return await mainContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
    }
}

