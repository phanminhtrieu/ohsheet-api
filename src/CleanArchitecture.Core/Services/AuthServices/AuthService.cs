using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Domain.Models.AuditLogins;
using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.AuditLoginServices;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Core.Interfaces.TokenService;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanArchitecture.Core.Services.AuthServices
{
    public class AuthService(
        UserManager<ApplicationUser> _userManager,
        IAuditLoginService _auditLoginService,
        ITokenService _tokenService, 
        IHttpContextAccessor _httpContextAccessor,
        ICookieService _cookieService) : IAuthService
    {
        public async Task<ApiResult<UserProfileResponse>> GetProfile(CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);;
            if (userId == null) throw UserException.UserUnauthorizedException();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw UserException.UserUnauthorizedException();

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null) throw UserException.InternalServerException();

            var respnse = new UserProfileResponse
            {
                UserName = user.UserName!,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                Role = roles.FirstOrDefault()!
            };

            return new ApiSuccessResult<UserProfileResponse>(respnse);
        }

        public bool Logout()
        {
            try
            {
                _ = _cookieService.Get();
                _cookieService.Delete();

                return true;
            }
            catch 
            {
                throw UserException.InternalServerException();
            }
        }

        public async Task<ApiResult<string>> RefreshToken()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier); ;
            if (userId == null) throw UserException.UserUnauthorizedException();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw UserException.InternalServerException();

            var accessToken = _tokenService.GenerateToken(user);
            _cookieService.Set(accessToken);

            return new ApiSuccessResult<string>(accessToken);
        }

        public async Task<ApiResult<UserSignInResponse>> SignIn(UserSignInRequest request)
        {
            var auditLoginRequest = new AuditLoginRequest()
            {
                UserId = string.Empty,
                UserName = request.UserName,
                IsSuccessded = false,
            };

            var auditLoginId = default(int);

            // Just can only signin by UserName
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                auditLoginRequest.Notes = "User not exist";
                auditLoginId = await this.SaveUserAuditLogin(auditLoginRequest);

                return new ApiErrorResult<UserSignInResponse>();
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword) 
            {
                auditLoginRequest.Notes = "Invalid password";
                auditLoginId = await this.SaveUserAuditLogin(auditLoginRequest);

                return new ApiResult<UserSignInResponse>();
            }

            auditLoginRequest.UserId = user.Id.ToString();
            auditLoginRequest.IsSuccessded = true;
            auditLoginRequest.Notes = "User sign in successfully";

            auditLoginId = await this.SaveUserAuditLogin(auditLoginRequest);

            var token = _tokenService.GenerateToken(user);
            _cookieService.Set(token);

            await this.UpdateUserLastSignInDate(user);

            var response = this.MapToResponse(user, token, auditLoginId);

            return new ApiSuccessResult<UserSignInResponse>(response);
        }

        public async Task<ApiResult<UserSignUpResponse>> SignUp(UserSignUpRequest request, CancellationToken cancellatoinToken)
        {
            var isUserNameExists = await _userManager.FindByNameAsync(request.UserName) != null;
            if (isUserNameExists) throw UserException.UserAlreadyExistsException("User Name");

            var isEmailExists = await _userManager.FindByEmailAsync(request.Email) != null;
            if (isEmailExists) throw UserException.UserAlreadyExistsException("Email");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Create user failed: {errors}");
            }

            var response = new UserSignUpResponse
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                //Role  = request.Role // TODO: Add enum role when signup
            };

            return new ApiSuccessResult<UserSignUpResponse>(response);
        }

        private async Task<int> SaveUserAuditLogin(AuditLoginRequest auditLogin)
        {
            var result = await _auditLoginService.AddAsync(auditLogin);
            if (!result.IsSucceeded) throw new Exception("Save audit login fail");

            return result.ResultObj;
        }

        private UserSignInResponse MapToResponse(ApplicationUser user, string token, int auditLoginId)
        {
            var response = new UserSignInResponse
            {
                UserName = user.UserName!,
                Email = user.Email,
                FirstName = user.FirstName,
                AuditLoginId = auditLoginId,
                Token = token
            };

            return response;
        }

        private async Task UpdateUserLastSignInDate(ApplicationUser user)
        {
            user.LastSignInDate = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) throw UserException.InternalServerException();
        }
    }
}
