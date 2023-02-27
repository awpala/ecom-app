using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
  public static class UserManagerExtensions
  {
    public static async Task<AppUser> FindUserByClaimsPrincipalWithAddress(
      this UserManager<AppUser> userManager,
      ClaimsPrincipal user
    )
    {
      var email = user.FindFirstValue(ClaimTypes.Email);
      var foundUser = await userManager.Users
        .Include(x => x.Address)
        .SingleOrDefaultAsync(x => x.Email == email);
      return foundUser;
    }

    public static async Task<AppUser> FindByEmailFromClaimsPrincipal(
      this UserManager<AppUser> userManager,
      ClaimsPrincipal user
    )
    {
      var foundUser = await userManager.Users
        .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
      return foundUser;
    }
  }
}
