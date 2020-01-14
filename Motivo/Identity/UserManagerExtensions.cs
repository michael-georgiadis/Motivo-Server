using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Motivo.Data;

namespace Motivo.Identity
{
    public static class UserManagerExtensions
    {
        public static async Task<MotivoUser> GetUserFromHttpContext(this UserManager<MotivoUser> userManager, HttpContext httpContext)
        {
            return await userManager.FindByNameAsync(httpContext.User?.Identity?.Name);
        }
    }
}
