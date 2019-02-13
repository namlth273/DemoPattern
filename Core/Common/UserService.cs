using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Common
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SystemUser = "System";
        public string CurrentUser { get; set; }

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            SetCurrentUser();
        }

        private void SetCurrentUser()
        {
            CurrentUser = SystemUser;
            //if (_httpContextAccessor == null)
            //{
            //    CurrentUser = SystemUser;
            //}
            //else
            //{
            //    if (_httpContextAccessor.HttpContext.User.HasClaim(h => h.Type == ClaimTypes.Email))
            //        CurrentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            //    if (string.IsNullOrEmpty(CurrentUser) && _httpContextAccessor.HttpContext.User.HasClaim(h => h.Type == ClaimTypes.NameIdentifier))
            //        CurrentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //    else
            //        CurrentUser = SystemUser;
            //}
        }
    }
}
