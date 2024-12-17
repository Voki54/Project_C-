using System.Security.Claims;

namespace Project_Manager.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
