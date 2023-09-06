using System;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.Abstractions.Token;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.DTOs;
using OnionArch.Application.DTOs.UserDTOs;
using OnionArch.Application.Features.Commands.AppUser.LoginUser;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Persistance.Repositorys.UserServices
{



    public class AuthService : IAuthService
    {


        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public AuthService(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
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
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

                return new LoginUserSuccessResponseDTO()
                {
                    token=token,
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

