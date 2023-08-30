using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.CustomExceptions;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandsHandle : IRequestHandler<LoginUserCommandsRequest, LoginUserCommandsResponse>
    {

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

        public LoginUserCommandsHandle(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandsResponse> Handle(LoginUserCommandsRequest request, CancellationToken cancellationToken)
        {
            //Kullanıcı adı varmı kontrol ediyoruz.
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
                //Kullanıcı adı ile ilgili bir bilgi bulamadıysak e mail kontrol ediyoruz.
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);

            if (user == null)
                //Kullanıcı adı ve Eposta ile ilgili bir bilgi bulamadıysak hata mesajı
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");

            //Burada ise bulunan kullanıcının veri tabanında olan şifresi ,
            //Form'dan gelen şifre ile aynı mı kontrol ediyoruz.
            //Eğer doğrulanmıyorsa kullanıcı kitlensin mi ? =>True False alanı

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                
            if (result.Succeeded) //Authentication başarılı!
            {
                //.... Yetkileri belirlememiz gerekiyor!
            }

            return new();
        }
    }
}

