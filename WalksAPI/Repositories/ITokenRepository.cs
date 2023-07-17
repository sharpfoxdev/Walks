using Microsoft.AspNetCore.Identity;

namespace WalksAPI.Repositories {
    public interface ITokenRepository {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
