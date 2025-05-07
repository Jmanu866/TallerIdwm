

using TallerIdwm.src.models;

namespace TallerIdwm.src.interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(User user, string role);
    }
}