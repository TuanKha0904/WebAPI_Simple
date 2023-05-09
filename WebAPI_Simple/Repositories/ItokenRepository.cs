using Microsoft.AspNetCore.Identity;

namespace WebAPI_Simple.Repositories
{
    public interface ItokenRepository
    {
        string CreateJWTToken (IdentityUser user, List<String> roles);
    }
}
