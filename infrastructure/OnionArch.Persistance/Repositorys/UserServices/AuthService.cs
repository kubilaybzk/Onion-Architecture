using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Token;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.CustomExceptions;
using OnionArch.Application.DTOs;
using OnionArch.Application.DTOs.UserDTOs;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Persistance.Repositorys.UserServices
{



    public class AuthService : IAuthService
    {


        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;
        readonly IUserService _userService;

        public AuthService(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
        }

        public Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            throw new NotImplementedException();
        }

        public Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginUserResponseDTO> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            //Kullanıcı adı varmı kontrol ediyoruz.
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                //Kullanıcı adı ile ilgili bir bilgi bulamadıysak e mail kontrol ediyoruz.
                user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                //Kullanıcı adı ve Eposta ile ilgili bir bilgi bulamadıysak hata mesajı
                //throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");

                return new LoginUserErrorResponseDTO()
                {
                    Message = "Kullanıcı bulunamadı",
                };

            //Burada ise bulunan kullanıcının veri tabanında olan şifresi ,
            //Form'dan gelen şifre ile aynı mı kontrol ediyoruz.
            //Eğer doğrulanmıyorsa kullanıcı kitlensin mi ? =>True False alanı

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) //Authentication başarılı!
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 2);
                return new LoginUserSuccessResponseDTO()
                {
                    token = token,
                    UserInfo = new()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        NameSurname = user.NameSurname

                    }

                };
            }
            else
            {
                return new LoginUserErrorResponseDTO()
                {
                    Message = "Kullanıcı adı ve şifre hatalı"
                };
            }
        }

        public async Task<LoginUserResponseDTO> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            //Kullanıcı var mı yok mu kontrol edelim .
           
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                //Kullanıcı var.

                Token token = _tokenHandler.CreateAccessToken(15,user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                //Yeni bir AccesToken yönlendirelim.
                return new LoginUserSuccessResponseDTO()
                {
                    token = token,
                    UserInfo = new()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        NameSurname = user.NameSurname

                    }

                };
            }
            else
            {
                return new LoginUserErrorResponseDTO()
                {
                    Message = "Kullanıcı adı ve şifre hatalı"
                };
            }
        }
    }
}

